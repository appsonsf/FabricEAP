using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConversationMsgStateService;
using InstantMessage;
using InstantMessage.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notification;
using ServiceFabricContrib;

namespace ConversationServiceTest.AppServices
{
    [TestClass]
    public class ConversationMsgAppServiceTest : TheAppServiceTestBase
    {
        [TestMethod]
        public async Task SendMessageAsync_Test()
        {
            //Arrage:
            await InitDictDataAsync();

            //Act:
            var service = new ConversationMsgAppService(statefulServiceContext, stateManager);
            var msg = DefaultConversationMsg.DeepCopy();
            await service.SendMessageAsync(msg);

            //Assert:
            using (var tx = stateManager.CreateTransaction())
            {
                var msgValue = await MessageProccessQueue.TryDequeueAsync(tx);
                var msg2 = msgValue.Value;
                Assert.IsTrue(msgValue.HasValue);
                Assert.IsTrue(msg2.Content == msg.Content);
            }
        }

        [TestMethod]
        public async Task GetMessagesAsync_Test()
        {
            //Arrage:
            await InitDictDataAsync();

            var service = new ConversationMsgAppService(statefulServiceContext, stateManager);
            var msg = DefaultConversationMsg.DeepCopy();

            //Act:
            var gettedMessages = await service.GetMessagesAsync(new List<MessageNotifyDto>()
            {
                new MessageNotifyDto()
                {
                    LatestMsgId = Guid.Empty,
                    Target = NotifyTargetType.Conversation,
                    TargetId = msg.ConversationId.ToString(),
                    TargetCategory = (int) ConversationType.P2P,
                }
            });

            //Assert:
            //PS:P2PMessageIndexDictName 类型错误
            Assert.AreEqual(2, gettedMessages.Count);
            var targetMsg = gettedMessages.Find(o => o.Id == msg.Id);
            Assert.IsNotNull(targetMsg);
            Assert.AreEqual(msg.Content, targetMsg.Content);
            Assert.AreEqual(msg.SenderId, targetMsg.SenderId);
            Assert.AreEqual(msg.Type, targetMsg.Type);
        }

        [TestMethod]
        public async Task GetOldMessagesAsync_Test()
        {
            //Arrage:
            await InitDictDataAsync();

            var service = new ConversationMsgAppService(statefulServiceContext, stateManager);
            var msg2 = DefaultConversationMsg2.DeepCopy();
            var msg1 = DefaultConversationMsg.DeepCopy();

            //Act:
            var oldMeesage = await service.GetOldMessagesAsync(new GetOldMessagesInput()
            {
                Id = msg2.ConversationId,
                OldestMsgId = msg2.Id,
            });

            //Assert:
            Assert.AreEqual(1, oldMeesage.Count);
        }

        /// <summary>
        /// 归档群组消息
        /// </summary>
        /// <returns></returns>
        //[TestMethod]
        //public async Task ArchiveGroupMessagesAsync_Test()
        //{
        //    //Arrage:
        //    await InitDicAsync();
        //    var service = new ConversationMsgAppService(statefulServiceContext, stateManager);

        //    //Act:群组消息会话id
        //    var conversationId = Guid.NewGuid();
        //    await service.ArchiveGroupMessagesAsync(conversationId);

        //    //Assert:
        //    using (var tx = stateManager.CreateTransaction())
        //    {
        //        var value = await pendingArchiveQueue.TryDequeueAsync(tx);
        //        Assert.IsTrue(value.HasValue);
        //        Assert.AreEqual(conversationId, value.Value);
        //    }
        //}
    }
}
