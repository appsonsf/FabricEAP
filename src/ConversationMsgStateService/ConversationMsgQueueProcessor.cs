using InstantMessage;
using InstantMessage.Entities;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Notification;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConversationMsgStateService
{
    public static class ConversationMsgQueueProcessor
    {
        public static bool IsInUnitTest = false;

        public static async Task ProcessQueueAsync(IReliableStateManager stateManager,
            IConversationCtrlAppService conversationCtrlAppService,
            Func<Guid, INotifySessionActor> notifySessionActorFactory,
            CancellationToken cancellationToken)
        {
            var queueMessageProcess = await Service.GetMessageProcessQueue(stateManager);

            while (!cancellationToken.IsCancellationRequested)
            {
                using (var tx = stateManager.CreateTransaction())
                {
                    var msgWrap = await queueMessageProcess.TryDequeueAsync(tx);
                    if (msgWrap.HasValue)
                    {
                        var item = msgWrap.Value;

                        await SaveMsgAsync(stateManager, tx, item);

                        await PushNotifyAsync(conversationCtrlAppService, notifySessionActorFactory, item);
                    }
                    else
                    {
                        await tx.CommitAsync();
                        await Task.Delay(100);
                    }
                }
            }
        }

        private static async Task SaveMsgAsync(IReliableStateManager stateManager,
            ITransaction tx, ConversationMsg item)
        {
            try
            {
                var listDict = await Service.GetMessageListDictByIdAsync(stateManager, item.Id);
                await listDict.AddAsync(tx, item.Id, item);

                var indexDict = await Service.GetMessageIndexDictByIdAsync(stateManager, item.ConversationId);
                var lstMessageIndex = await indexDict.GetOrAddAsync(tx, item.ConversationId, new List<Guid>());
                lstMessageIndex.Add(item.Id);
                await indexDict.SetAsync(tx, item.ConversationId, lstMessageIndex);

                await tx.CommitAsync();
            }
            catch (Exception ex)
            {
                Log.Error("ConversationMsgQueueProcessor SaveMsgAsync Error: " +
                    $"ItemId: {item.Id}; ConversationId: {item.ConversationId}; SenderId: {item.SenderId}; Message: {ex.Message}");
                if (IsInUnitTest) throw;
            }
        }

        private static async Task PushNotifyAsync(
            IConversationCtrlAppService conversationCtrlAppService,
            Func<Guid, INotifySessionActor> notifySessionActorFactory,
            ConversationMsg item)
        {
            try
            {
                var entityConversationWrap = await conversationCtrlAppService.GetByIdAsync(item.ConversationId);
                if (entityConversationWrap.HasValue)
                {
                    var entityConversation = entityConversationWrap.Value;

                    var tasks = new List<Task>();
                    foreach (var userId in entityConversation.Participants)
                    {
                        var actorSession = notifySessionActorFactory(userId);
                        tasks.Add(actorSession.PushMsgNotifyAsync(new MessageNotifyDto
                        {
                            Target = NotifyTargetType.Conversation,
                            TargetId = item.ConversationId.ToString(),
                            TargetCategory = (int)entityConversation.Type,
                            LatestMsgId = item.Id,
                        }));
                    }
                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception ex)
            {
                //如果PushMsgNotifyAsync过程出错，也实现保存消息的过程，只是会出现接收方接收不到消息
                Log.Error("ConversationMsgQueueProcessor PushMsgNotifyAsync Message: " +
                    $"ItemId is {item.Id}, ConversationId is {item.ConversationId}, SenderId is {item.SenderId}, Message is {ex.Message}");
                if (IsInUnitTest) throw;
            }
        }
    }
}
