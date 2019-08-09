using InstantMessage.Entities;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ConversationMsgStateService.ConversationMsgAppService;

namespace ConversationMsgStateService
{
    public static class ArchiveGroupQueueProcessor
    {
        //public static async Task ProcessAsync(IReliableStateManager stateManager,
        //    IProcessorConfigurator configurator,
        //    CancellationToken cancellationToken)
        //{
        //    var queue = await Service.GetPendingArchiveGroupQueue(stateManager);
        //    var dictGroupMessagesIndices = await stateManager.GetOrAddAsync<IReliableDictionary2<Guid, List<Guid>>>(GroupMessageIndexDictName);
        //    var dictGroupMessages = await stateManager.GetOrAddAsync<IReliableDictionary2<Guid, ConversationMsg>>(GroupMessageDictName);
        //    var dictArchiveGroupMessages = await stateManager.GetOrAddAsync<IReliableDictionary2<Guid, List<ConversationMsg>>>(ArchivedGroupMessageDictName);

        //    while (!cancellationToken.IsCancellationRequested)
        //    {
        //        if (configurator.IsArchiveGroupProcessTime())
        //        {
        //            bool doDelay = false;
        //            using (var tx = stateManager.CreateTransaction())
        //            {
        //                var idWrap = await queue.TryDequeueAsync(tx);
        //                doDelay = !idWrap.HasValue;
        //                if (idWrap.HasValue)
        //                {
        //                    var id = idWrap.Value;

        //                    var msgIdsWrap = await dictGroupMessagesIndices.TryGetValueAsync(tx, id);
        //                    if (msgIdsWrap.HasValue)
        //                    {
        //                        var lstMsg = new List<ConversationMsg>();
        //                        var msgIds = msgIdsWrap.Value;
        //                        var enumerator = (await dictGroupMessages.CreateEnumerableAsync(tx,
        //                            o => msgIds.Contains(o), EnumerationMode.Unordered)).GetAsyncEnumerator();
        //                        while (await enumerator.MoveNextAsync(cancellationToken))
        //                        {
        //                            lstMsg.Add(enumerator.Current.Value);
        //                        }

        //                        //remove msg from GroupMessageDict
        //                        foreach (var msg in lstMsg)
        //                        {
        //                            await dictGroupMessages.TryRemoveAsync(tx, msg.Id);
        //                        }

        //                        //add archived msg into ArchivedGroupMessageDict
        //                        await dictArchiveGroupMessages.TryAddAsync(tx, id, lstMsg);

        //                        await tx.CommitAsync();
        //                    }
        //                }
        //            }
        //            if (doDelay) await Task.Delay(TimeSpan.FromMinutes(3));
        //        }
        //        else
        //            await Task.Delay(TimeSpan.FromSeconds(30));
        //    }
        //}
    }
}
