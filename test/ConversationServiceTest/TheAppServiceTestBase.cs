using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstantMessage;
using InstantMessage.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.ServiceFabric.Data.Collections;
using ServiceFabricContrib;
using UnitTestCommon;
using Common;

namespace ConversationServiceTest
{
    public class TheAppServiceTestBase : AppServiceTestBase
    {
        protected IReliableDictionary2<string, Guid> dictKeyToId { get; set; }
        protected IReliableDictionary2<Guid, Conversation> dictIdToEntity { get; set; }
        protected static Guid SenderId = Guid.NewGuid();
        private static Guid ReciverId = Guid.NewGuid();
        protected static Conversation DefaultConversation { get; set; } = new Conversation()
        {
            Id = Guid.NewGuid(),
            Key = Guid.NewGuid().ToString("N").ComputeMd5(),
            IsDeleted = false,
            Type = ConversationType.P2P,
            Updated = DateTimeOffset.Now,
            Participants = new List<Guid>() { SenderId, ReciverId }
        };
        protected static ConversationMsg DefaultConversationMsg { get; set; } = new ConversationMsg()
        {
            Id = Guid.NewGuid(),
            ConversationId = DefaultConversation.Id,
            Content = "hello,world",
            SenderId = SenderId,
            Time = DateTimeOffset.Now,
            Type = ConversationMsgType.Text
        };
        protected static ConversationMsg DefaultConversationMsg2 { get; set; } = new ConversationMsg()
        {
            Id = Guid.NewGuid(),
            ConversationId = DefaultConversation.Id,
            Content = "hello,world2",
            SenderId = SenderId,
            Time = DateTimeOffset.Now,
            Type = ConversationMsgType.Text
        };

      
        protected IReliableConcurrentQueue<ConversationMsg> MessageProccessQueue { get; set; }

        protected IReliableDictionary2<Guid, List<Guid>> MessageIndexDict { get; set; }
        protected IReliableDictionary2<Guid, ConversationMsg> MessageListDict { get; set; }

        private async Task InitDictAsync()
        {
            dictKeyToId = await ConversationCtrlStateService.Service.GetKeyToIdDictAsync(stateManager);
            dictIdToEntity = await ConversationCtrlStateService.Service.GetListDictAsync(stateManager);

            MessageProccessQueue = await ConversationMsgStateService.Service.GetMessageProcessQueue(stateManager);
            MessageIndexDict = await ConversationMsgStateService.Service.GetMessageIndexDictByIdAsync(stateManager, DefaultConversation.Id);
            MessageListDict = await ConversationMsgStateService.Service.GetMessageListDictByIdAsync(stateManager, DefaultConversationMsg.Id);
        }

        protected async Task InitDictDataAsync()
        {
            await InitDictAsync();

            var conversation = DefaultConversation.DeepCopy(); //need deep copy
            var message1 = DefaultConversationMsg.DeepCopy();
            var message2 = DefaultConversationMsg2.DeepCopy();
            using (var tx = stateManager.CreateTransaction())
            {
                await dictKeyToId.AddOrUpdateAsync(tx, conversation.Key, conversation.Id, (k, v) => conversation.Id);
                await dictIdToEntity.AddOrUpdateAsync(tx, conversation.Id, conversation, (k, v) => conversation);

                await MessageIndexDict.SetAsync(tx, message1.ConversationId, new List<Guid>() { message1.Id, message2.Id });
                await MessageListDict.SetAsync(tx, message1.Id, message1);
                await MessageListDict.SetAsync(tx, message2.Id, message2);

                await tx.CommitAsync();
            }
        }

        protected async Task InitQueueDataAsync()
        {
            await InitDictAsync();

            var message1 = DefaultConversationMsg.DeepCopy();
            using (var tx = stateManager.CreateTransaction())
            {
                await MessageProccessQueue.EnqueueAsync(tx, message1);

                await tx.CommitAsync();
            }
        }

        //public async Task InitArchivedAndPendingMessageQueueAsync()
        //{
        //    using (var tx = stateManager.CreateTransaction())
        //    {
        //        var msg = DefaultPendingMsg.DeepCopy();
        //        //pre-init pending a Group meesage
        //        await pendingArchiveQueue.EnqueueAsync(tx, msg.ConversationId);
        //        await dictGroupMessagesIndices.AddAsync(tx, msg.ConversationId, new List<Guid>() { msg.Id });
        //        await dictGroupMessages.AddAsync(tx, msg.Id, msg);
        //        await tx.CommitAsync();
        //    }
        //}
    }
}
