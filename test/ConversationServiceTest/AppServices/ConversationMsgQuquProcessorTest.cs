using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConversationMsgStateService;
using InstantMessage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Notification;
using ServiceFabricContrib;
using UnitTestCommon;

namespace ConversationServiceTest.AppServices
{
    [TestClass]
    public class ConversationMsgQuquProcessorTest : TheAppServiceTestBase
    {
        [TestMethod]
        public async Task ProcessQueueAsync_Test()
        {
            //Arrage:
            await InitQueueDataAsync();

            var conversation = DefaultConversation.DeepCopy();
            var mock_conversationCtrlAppService = new Mock<IConversationCtrlAppService>();
            var mock_notifySessionActor = new Mock<INotifySessionActor>();
            mock_conversationCtrlAppService.Setup(u => u.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            mock_conversationCtrlAppService.Setup(u => u.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(conversation);
            mock_notifySessionActor.Setup(u => u.PushMsgNotifyAsync(It.IsAny<MessageNotifyDto>())).Returns(Task.CompletedTask);
            Func<Guid, INotifySessionActor> notifySessionActorFactory = (id) => mock_notifySessionActor.Object;

            //Act:
            CancellationTokenSource source = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(u =>
            {
                Thread.Sleep(200);
                source.Cancel();
            });
            
            ConversationMsgQueueProcessor.IsInUnitTest = true;
            await ConversationMsgQueueProcessor.ProcessQueueAsync(stateManager,
                mock_conversationCtrlAppService.Object, notifySessionActorFactory, source.Token);

            //Assert:
            var msg1 = DefaultConversationMsg.DeepCopy();
            using (var tx = stateManager.CreateTransaction())
            {
                Assert.AreEqual(0, MessageProccessQueue.Count);
                var msgIndex = await MessageIndexDict.TryGetValueAsync(tx, msg1.ConversationId);
                var msgIds = msgIndex.Value;
                var msg = await MessageListDict.TryGetValueAsync(tx, msgIds[0]);
                Assert.IsTrue(msgIndex.HasValue);
                Assert.AreEqual(1, msgIds.Count);
                Assert.AreEqual(msg1, msg.Value);
                mock_conversationCtrlAppService.Verify(u => u.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
                mock_notifySessionActor.Verify(u => u.PushMsgNotifyAsync(It.IsAny<MessageNotifyDto>()), Times.AtLeast(2));
            }
        }
    }
}
