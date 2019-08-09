using ConfigMgmt;
using ConfigMgmt.Entities;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

namespace SystemConfigStateService
{
    public class BizSystemAppService : StatefulRemotingService, IBizSystemAppService
    {
        public BizSystemAppService(StatefulServiceContext serviceContext, IReliableStateManager stateManager) :
            base(serviceContext, stateManager)
        {
        }

        public async Task<bool> AddOrUpdateForMsgNotifyAsync(MsgNotifyBizSystem input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var dict = await StateManager.GetOrAddAsync<IReliableDictionary2<string, MsgNotifyBizSystem>>(Service.DictionaryName_MsgNotifyBizSystem);
            using (var tx = StateManager.CreateTransaction())
            {
                await dict.AddOrUpdateAsync(tx, input.Id, input, (k, v) => input);

                await tx.CommitAsync();
                return true;
            }
        }

        public async Task<bool> AddOrUpdateForTodoCenterAsync(TodoCenterBizSystem input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var dict = await StateManager.GetOrAddAsync<IReliableDictionary2<string, TodoCenterBizSystem>>(Service.DictionaryName_TodoCenterBizSystem);
            using (var tx = StateManager.CreateTransaction())
            {
                await dict.AddOrUpdateAsync(tx, input.Id, input, (k, v) => input);

                await tx.CommitAsync();
                return true;
            }
        }

        public async Task<List<MsgNotifyBizSystem>> GetAllForMsgNotifyAsync()
        {
            var token = CancellationToken.None;
            var dict = await StateManager.GetOrAddAsync<IReliableDictionary2<string, MsgNotifyBizSystem>>(Service.DictionaryName_MsgNotifyBizSystem);
            using (var tx = StateManager.CreateTransaction())
            {
                return await dict.GetAllAsync(tx, token);
            }
        }

        public async Task<List<TodoCenterBizSystem>> GetAllForTodoCenterAsync()
        {
            var token = CancellationToken.None;
            var dict = await StateManager.GetOrAddAsync<IReliableDictionary2<string, TodoCenterBizSystem>>(Service.DictionaryName_TodoCenterBizSystem);
            using (var tx = StateManager.CreateTransaction())
            {
                return await dict.GetAllAsync(tx, token);
            }
        }

        public async Task<MsgNotifyBizSystem> GetByIdForNotifyMsgAsync(string id)
        {
            var token = CancellationToken.None;
            var dict = await StateManager.GetOrAddAsync<IReliableDictionary2<string, MsgNotifyBizSystem>>(Service.DictionaryName_MsgNotifyBizSystem);
            using (var tx = StateManager.CreateTransaction())
            {
                var wrap= await dict.TryGetValueAsync(tx, id);
                if (wrap.HasValue) return wrap.Value;
            }
            return null;
        }

        public async Task<bool> RemoveForMsgNotifyAsync(string id)
        {
            var dict = await StateManager.GetOrAddAsync<IReliableDictionary2<string, MsgNotifyBizSystem>>(Service.DictionaryName_MsgNotifyBizSystem);
            using (var tx = StateManager.CreateTransaction())
            {
                var result = await dict.TryRemoveAsync(tx, id);

                await tx.CommitAsync();
                return result.HasValue;
            }
        }

        public async Task<bool> RemoveForTodoCenterAsync(string id)
        {
            var dict = await StateManager.GetOrAddAsync<IReliableDictionary2<string, TodoCenterBizSystem>>(Service.DictionaryName_TodoCenterBizSystem);
            using (var tx = StateManager.CreateTransaction())
            {
                var result = await dict.TryRemoveAsync(tx, id);

                await tx.CommitAsync();
                return result.HasValue;
            }
        }
    }
}
