using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConversationCtrlStateService;
using EnterpriseContact;
using FluentAssertions;
using InstantMessage;
using InstantMessage.Entities;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Notification;
using NSubstitute;
using ServiceFabricContrib;

namespace ConversationServiceTest.AppServices
{
    [TestClass]
    public class ConversationCtrlStateServiceTest : TheAppServiceTestBase
    {
        [TestMethod]
        public async Task GetByIdAsync_Test()
        {
            //Arrage:
            await InitDictDataAsync();

            //Act:
            var service = new ConversationCtrlAppService(statefulServiceContext, stateManager, Substitute.For<IEmployeeAppService>());
            var conversation = await service.GetByIdAsync(DefaultConversation.Id);

            //Assert:
            Assert.IsTrue(conversation.HasValue);
            Assert.IsFalse(conversation.Value.IsDeleted);
            Assert.AreEqual(2, conversation.Value.Participants.Count);
            Assert.AreEqual(ConversationType.P2P, conversation.Value.Type);
            Assert.AreEqual(DefaultConversation.Key, conversation.Value.Key);

        }

        [TestMethod]
        public async Task AddP2PConversationAsync_Test()
        {
            //Arrage:
            await InitDictDataAsync();

            var senderId = Guid.NewGuid();
            var reciverId = Guid.NewGuid();

            //Act:
            var service = new ConversationCtrlAppService(statefulServiceContext, stateManager, Substitute.For<IEmployeeAppService>());
            await service.GetOrAddP2PAsync(senderId, reciverId);
            var key = ConversationCtrlAppService.GenerateConversationKey(senderId, reciverId);

            //Assert:
            Assert.IsNotNull(key);
            using (var tx = stateManager.CreateTransaction())
            {
                var keyToId = await dictKeyToId.TryGetValueAsync(tx, key);
                var idToEntity = await dictIdToEntity.TryGetValueAsync(tx, keyToId.Value);
                var conversation = idToEntity.Value;

                Assert.IsNotNull(keyToId);
                Assert.IsTrue(idToEntity.HasValue);
                Assert.IsFalse(conversation.IsDeleted);

                Assert.IsTrue(conversation.Type == ConversationType.P2P);
                Assert.AreEqual(2, conversation.Participants.Count);
                Assert.IsTrue(conversation.Participants.Contains(senderId));
                Assert.IsTrue(conversation.Participants.Contains(reciverId));
            }

        }


        [TestMethod]
        public async Task DeleteAsync_Test()
        {
            //Arrage:
            await InitDictDataAsync();

            //Act:
            var service = new ConversationCtrlAppService(statefulServiceContext, stateManager, Substitute.For<IEmployeeAppService>());
            await service.DeleteAsync(DefaultConversation.Id);
            await service.DeleteAsync(Guid.NewGuid());

            //Assert:
            using (var tx = stateManager.CreateTransaction())
            {
                var idToEntity = await dictIdToEntity.TryGetValueAsync(tx, DefaultConversation.Id);
                var keyToId = await dictKeyToId.TryGetValueAsync(tx, DefaultConversation.Key);
                Assert.IsTrue(idToEntity.HasValue);
                Assert.IsTrue(idToEntity.Value.IsDeleted);
                Assert.IsFalse(keyToId.HasValue);
            }
        }

        [TestMethod]
        public async Task AddDepAsync()
        {
            //Arrage:
            await InitDictDataAsync();

            var depId = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();

            var employeeAppService = Substitute.For<IEmployeeAppService>();
            employeeAppService.GetUserIdsByDepartmentIdAsync(depId)
                .Returns(new List<Guid> { userId1, userId2 });

            //Act:
            var service = new ConversationCtrlAppService(statefulServiceContext, stateManager, employeeAppService);
            var result = await service.AddDepAsync(new AddDepConversationInput { UserId = userId1, DepartmentId = depId });

            //Assert:
            result.IsSuccess.Should().BeTrue();
            await employeeAppService.Received(1).GetUserIdsByDepartmentIdAsync(depId);

            //Fail(1)
            employeeAppService.GetUserIdsByDepartmentIdAsync(depId)
               .Returns(new List<Guid> { Guid.NewGuid(), userId2 });

            result = await service.AddDepAsync(new AddDepConversationInput { UserId = userId1, DepartmentId = depId });
            result.FailedCode.Should().Be(1);


            //Fail(2)
            employeeAppService.GetUserIdsByDepartmentIdAsync(depId)
               .Returns(new List<Guid> { userId1 });

            result = await service.AddDepAsync(new AddDepConversationInput { UserId = userId1, DepartmentId = depId });
            result.FailedCode.Should().Be(2);
        }
    }
}
