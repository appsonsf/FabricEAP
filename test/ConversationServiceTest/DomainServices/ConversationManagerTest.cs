using ConversationCtrlStateService;
using ConversationCtrlStateService.DomainServices;
using InstantMessage;
using InstantMessage.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnitTestCommon;

namespace ConversationServiceTest.DomainServices
{
    [TestClass]
    public class ConversationManagerTest: ManagerTestBase
    {
        [TestMethod]
        public async Task AddOrUpdateAsync()
        {
            var target = new ConversationManager(stateManager);

            var id = Guid.NewGuid();

            await target.AddOrUpdateAsync(id, 
                new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), }, 
                ConversationType.CustomGroup);

            //Assert:
            using (var tx = stateManager.CreateTransaction())
            {
                var dictIdToEntity = await Service.GetListDictAsync(stateManager);

                var idToEntity = await dictIdToEntity.TryGetValueAsync(tx, id);
                var conversation = idToEntity.Value;
                Assert.IsTrue(idToEntity.HasValue);
                Assert.IsTrue(conversation.Type == ConversationType.CustomGroup);
                Assert.IsTrue(conversation.Participants.Count == 2);
            }
        }
    }
}
