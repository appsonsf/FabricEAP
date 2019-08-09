using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using MassTransit;
using Microsoft.Diagnostics.EventFlow;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Notification;
using Notification.MsgContracts;
using NotifySessionActor;
using NSubstitute;
using ServiceFabric.Mocks;
using UnitTestCommon;

namespace NotifySessionActorTest
{
    [TestClass]
    public class NotifySessionActorTest : TheActorTestBase
    {
        [TestMethod]
        public async Task Test_AddConnectionIdAsync_RemoveConnectionIdAsync()
        {
            var userId = Guid.NewGuid();
            var connectionId = Guid.NewGuid().ToString("N");

            var actor = new TheActor(CreateActorService(CreateBus(null)), new ActorId(userId));
            actor.IsInUnitTest = true;

            await actor.AddConnectionIdAsync(connectionId);
            var connectionIds = await actorStateManager.GetStateAsync<List<string>>(TheActor.ConnectionIdListPropertyName);
            Assert.IsTrue(connectionIds.Count == 1);  //添加成功

            await actor.RemoveConnectionIdAsync(Guid.NewGuid().ToString("N"));
            connectionIds = await actorStateManager.GetStateAsync<List<string>>(TheActor.ConnectionIdListPropertyName);
            Assert.IsTrue(connectionIds.Count == 1); //没有被删除掉

            await actor.RemoveConnectionIdAsync(connectionId);
            connectionIds = await actorStateManager.GetStateAsync<List<string>>(TheActor.ConnectionIdListPropertyName);
            Assert.IsTrue(connectionIds == null || connectionIds.Count == 0); //删除掉
        }

        [TestMethod]
        public async Task Test_PushEventNotifyAsync_WhenHaveConnection()
        {
            var userId = Guid.NewGuid();
            var connectionId = Guid.NewGuid().ToString("N");
            var conversationId = Guid.NewGuid();

            void configBus(IInMemoryReceiveEndpointConfigurator e)
            {
                e.Handler<SendEventNotify>(context =>
                {
                    var notify = context.Message;

                    Assert.IsNotNull(notify);
                    Assert.AreEqual(1, notify.Notifies.Count);
                    Assert.AreEqual(conversationId.ToString(), notify.Notifies[0].TargetId);
                    Assert.AreEqual(1, notify.Notifies[0].TargetCategory);
                    Assert.AreEqual("Test", notify.Notifies[0].Text);

                    return Task.CompletedTask;
                });

                MapEndpointConverion(e);
            };

            var actor = new TheActor(CreateActorService(CreateBus(configBus)), new ActorId(userId));
            actor.IsInUnitTest = true;

            await actor.AddConnectionIdAsync(connectionId);
            await actor.PushEventNotifyAsync(new EventNotifyDto
            {
                Target = NotifyTargetType.Conversation,
                TargetId = conversationId.ToString(),
                Text = "Test",
                TargetCategory = 1
            });
            await Task.Delay(500);
        }

        private static void MapEndpointConverion(IInMemoryReceiveEndpointConfigurator e)
        {
            try
            {
                EndpointConvention.Map<SendMessageNotify>(e.InputAddress);
                EndpointConvention.Map<SendEventNotify>(e.InputAddress);
            }
            catch
            {
            }
        }

        [TestMethod]
        public async Task Test_PushMsgNotifyAsync_WhenHaveConnection()
        {
            var userId = Guid.NewGuid();
            var connectionId = Guid.NewGuid().ToString("N");
            var conversationId = Guid.NewGuid();
            var msgId = Guid.NewGuid();

            void configBus(IInMemoryReceiveEndpointConfigurator e)
            {
                e.Handler<SendMessageNotify>(context =>
                {
                    var notify = context.Message;

                    Assert.IsNotNull(notify);
                    Assert.AreEqual(1, notify.Notifies.Count);
                    Assert.AreEqual(conversationId.ToString(), notify.Notifies[0].TargetId);
                    Assert.AreEqual(msgId, notify.Notifies[0].LatestMsgId);
                    Assert.AreEqual(1, notify.Notifies[0].TargetCategory);

                    return Task.CompletedTask;
                });

                MapEndpointConverion(e);
            };

            var actor = new TheActor(CreateActorService(CreateBus(configBus)), new ActorId(userId));
            actor.IsInUnitTest = true;

            await actor.AddConnectionIdAsync(connectionId);
            await actor.PushMsgNotifyAsync(new MessageNotifyDto
            {
                Target = NotifyTargetType.Conversation,
                TargetId = conversationId.ToString(),
                TargetCategory = 1,
                LatestMsgId = msgId,
            });
            await Task.Delay(500);
        }

        [TestMethod]
        public async Task Tesh_PushMsgNotifyAsync_WhenNotHaveConnection()
        {
            var userId = Guid.NewGuid();
            var conversationId = Guid.NewGuid();
            var msgId = Guid.NewGuid();

            void configBus(IInMemoryReceiveEndpointConfigurator e)
            {
                e.Handler<SendMessageNotify>(context =>
                {
                    var notify = context.Message;

                    Assert.Fail();

                    return Task.CompletedTask;
                });

                MapEndpointConverion(e);
            };

            var actor = new TheActor(CreateActorService(CreateBus(configBus)), new ActorId(userId));
            actor.IsInUnitTest = true;

            await actor.PushMsgNotifyAsync(new MessageNotifyDto
            {
                Target = NotifyTargetType.Conversation,
                TargetId = conversationId.ToString(),
                TargetCategory = 1,
                LatestMsgId = msgId,
            });
            var listPushNotify = await actorStateManager.GetStateAsync<List<MessageNotifyDto>>(
                TheActor.MessageListPropertyName);
            Assert.IsTrue(listPushNotify.Count > 0);
            var dto = listPushNotify.Find(o => o.TargetId == conversationId.ToString());
            Assert.IsNotNull(dto);
            Assert.AreEqual(msgId, dto.LatestMsgId);
        }
    }
}
