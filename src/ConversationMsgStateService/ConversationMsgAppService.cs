using InstantMessage;
using InstantMessage.Entities;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Notification;
using Serilog;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConversationMsgStateService
{
    public class ConversationMsgAppService : StatefulRemotingService, IConversationMsgAppService
    {
        public ConversationMsgAppService(StatefulServiceContext serviceContext,
            IReliableStateManager stateManager) : base(serviceContext, stateManager)
        {
            _logger = Log.ForContext<ConversationMsgAppService>();
        }

        private const int MaxMessageAmount = 20;
        private readonly ILogger _logger;


        private static async Task<List<Guid>> BatchGetMsgIds(List<MessageNotifyDto> input,
            List<IReliableDictionary2<Guid, List<Guid>>> indexDicts, ITransaction tx)
        {
            var willBatchGetMsgIds = new List<Guid>();
            foreach (var indexDict in indexDicts)
            {
                foreach (var notify in input)
                {
                    var id = notify.TargetId.ToGuid();
                    if (await indexDict.ContainsKeyAsync(tx, id))
                    {
                        var lstMessageIndex = (await indexDict.TryGetValueAsync(tx, id)).Value;
                        var latestMsgIndex = notify.LatestMsgId == Guid.Empty
                            ? lstMessageIndex.Count - MaxMessageAmount
                            : lstMessageIndex.IndexOf(notify.LatestMsgId);
                        if (latestMsgIndex < 0) latestMsgIndex = 0;
                        var willGetMsgIds = (lstMessageIndex.Count - latestMsgIndex) > MaxMessageAmount
                            ? lstMessageIndex.GetRange(lstMessageIndex.Count - MaxMessageAmount, MaxMessageAmount)
                            : lstMessageIndex.GetRange(latestMsgIndex, lstMessageIndex.Count - latestMsgIndex);

                        willBatchGetMsgIds.AddRange(willGetMsgIds);
                    }
                }
            }

            return willBatchGetMsgIds;
        }

        public async Task<List<ConversationMsg>> GetMessagesAsync(List<MessageNotifyDto> input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            _logger.Debug("GetMessagesAsync, conversationIds is {ids}", string.Join(",", input.Select(o => o.TargetId)));

            var indexDicts = await Service.GetAllMessageIndexDictsAsync(StateManager);
            var listDicts = await Service.GetAllMessageListDictsAsync(StateManager);
            var token = CancellationToken.None;
            var result = new List<ConversationMsg>();
            using (var tx = StateManager.CreateTransaction())
            {
                var willBatchGetMsgIds = await BatchGetMsgIds(input, indexDicts, tx);

                foreach (var listDict in listDicts)
                {
                    result.AddRange(await listDict.GetAllByIdsAsync(tx, willBatchGetMsgIds, token));
                }
            }

            _logger.Debug("GetMessagesAsync msg count is {count}", result.Count);
            return result;
        }

        private static async Task<List<Guid>> GetOldGetMsgIds(GetOldMessagesInput input,
          IReliableDictionary2<Guid, List<Guid>> indexDict, ITransaction tx)
        {
            if (await indexDict.ContainsKeyAsync(tx, input.Id))
            {
                var lstMessageIndex = (await indexDict.TryGetValueAsync(tx, input.Id)).Value;
                var oldestMsgIndex = lstMessageIndex.IndexOf(input.OldestMsgId);
                if (oldestMsgIndex == 0) return null;

                var endIndex = oldestMsgIndex - 1;
                var startIndex = oldestMsgIndex - MaxMessageAmount;
                var amount = MaxMessageAmount;
                if (startIndex < 0)
                {
                    startIndex = 0;
                    amount = endIndex - startIndex + 1;
                }

                return lstMessageIndex.GetRange(startIndex, amount);
            }

            return null;
        }

        public async Task<List<ConversationMsg>> GetOldMessagesAsync(GetOldMessagesInput input)
        {
            if (input.Id == Guid.Empty) throw new ArgumentException("ConversationId is empty", nameof(input.Id));
            if (input.OldestMsgId == Guid.Empty) throw new ArgumentException("OldestMsgId is empty", nameof(input.OldestMsgId));

            _logger.Debug("GetOldMessagesAsync, conversationId is {id}", input.Id);

            var result = new List<ConversationMsg>();
            var token = CancellationToken.None;
            var indexDict = await Service.GetMessageIndexDictByIdAsync(StateManager, input.Id);
            var listDicts = await Service.GetAllMessageListDictsAsync(StateManager);

            using (var tx = StateManager.CreateTransaction())
            {
                var willOldGetMsgIds = await GetOldGetMsgIds(input, indexDict, tx);
                if (willOldGetMsgIds == null) return new List<ConversationMsg>();

                foreach (var listDict in listDicts)
                {
                    result.AddRange(await listDict.GetAllByIdsAsync(tx, willOldGetMsgIds, token));
                }
            }

            _logger.Debug("GetOldMessagesAsync, msg count is {count}", result.Count);
            return result;
        }

        public async Task SendMessageAsync(ConversationMsg msg)
        {
            var queue = await Service.GetMessageProcessQueue(StateManager);
            using (var tx = StateManager.CreateTransaction())
            {
                _logger.Debug("Enqueue ConversationMsg: {msgId}", msg.Id);

                await queue.EnqueueAsync(tx, msg);
                await tx.CommitAsync();
            }
        }

        public async Task ArchiveGroupMessagesAsync(Guid id)
        {
            var queue = await Service.GetPendingArchiveGroupQueue(StateManager);
            using (var tx = StateManager.CreateTransaction())
            {
                _logger.Debug("Enqueue ConversationId: {id}", id);

                await queue.EnqueueAsync(tx, id);
                await tx.CommitAsync();
            }
        }
    }
}
