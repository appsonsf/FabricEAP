using AutoMapper;
using ConfigMgmt;
using ConfigMgmt.Entities;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SystemConfigStateService
{
    public class WorkbenchAppService : StatefulRemotingService, IWorkbenchAppService
    {
        private readonly IMapper _mapper;

        public WorkbenchAppService(StatefulServiceContext serviceContext, IReliableStateManager stateManager
            , IMapper mapper)
         : base(serviceContext, stateManager)
        {
            _mapper = mapper;
        }

        public async Task<bool> AddOrUpdateEntrancesAsync(AddEntrancesInput input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (string.IsNullOrEmpty(input.ClientId))
                throw new ArgumentNullException(nameof(input.ClientId));

            if (input.AppEntrances == null || input.AppEntrances.Count == 0)
                throw new ArgumentNullException(nameof(input.AppEntrances));

            foreach (var item in input.AppEntrances)
            {
                if (item.ClientId != input.ClientId)
                    throw new ArgumentException("所有AppEntrance的ClientId必须等于clientId", nameof(input.AppEntrances));
            }

            var dictClients = await StateManager.GetOrAddAsync<IReliableDictionary2<string, Client>>(Service.DictionaryName_Client);
            var dictAppEntrances = await StateManager.GetOrAddAsync<IReliableDictionary2<Guid, AppEntrance>>(Service.DictionaryName_AppEntrance);
            using (var tx = StateManager.CreateTransaction())
            {
                var clientWrap = await dictClients.TryGetValueAsync(tx, input.ClientId);
                if (!clientWrap.HasValue) return false;
                var client = clientWrap.Value;
                foreach (var item in input.AppEntrances)
                {
                    await dictAppEntrances.AddOrUpdateAsync(tx, item.Id, item, (k, o) => item);
                    if (!client.AppEntranceIds.Contains(item.Id))
                        client.AppEntranceIds.Add(item.Id);
                }
                await dictClients.TryUpdateAsync(tx, input.ClientId, client, clientWrap.Value);

                await tx.CommitAsync();
                return true;
            }
        }

        public async Task<List<AppEntranceDto>> GetAppEntrancesAsync(GetAppEntrancesInput input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (string.IsNullOrEmpty(input.ClientId))
                throw new ArgumentNullException(nameof(input.ClientId));

            var dictClients = await StateManager.GetOrAddAsync<IReliableDictionary2<string, Client>>(Service.DictionaryName_Client);
            var dictAppEntrances = await StateManager.GetOrAddAsync<IReliableDictionary2<Guid, AppEntrance>>(Service.DictionaryName_AppEntrance);
            var token = CancellationToken.None;
            var result = new List<AppEntranceDto>();
            using (var tx = StateManager.CreateTransaction())
            {
                var clientWrap = await dictClients.TryGetValueAsync(tx, input.ClientId);
                if (!clientWrap.HasValue)
                    throw new ArgumentOutOfRangeException("没有clientId的数据", nameof(input.ClientId));

                var client = clientWrap.Value;
                var enumerator = (await dictAppEntrances.CreateEnumerableAsync(tx,
                          o => client.AppEntranceIds.Contains(o), EnumerationMode.Unordered)).GetAsyncEnumerator();
                while (await enumerator.MoveNextAsync(token))
                {
                    var item = enumerator.Current.Value;
                    if (item.NeedRoles?.Length > 0)
                    {
                        //without any roles then continue
                        if (input.UserRoles == null || input.UserRoles.Count == 0) continue;
                        var hasNeedRole = true;
                        foreach (var role in item.NeedRoles)
                        {
                            //without this role then break
                            hasNeedRole = input.UserRoles.Contains(role);
                            if (!hasNeedRole) break;
                        }
                        if (!hasNeedRole) continue;
                    }
                    var dto = CheckPlatformThenAdd(input.ClientPlatform, result, item);
                }
            }
            return result;
        }

        private AppEntranceDto CheckPlatformThenAdd(ClientPlatform clientPlatform, List<AppEntranceDto> result, AppEntrance item)
        {
            if(item.IsFolder)
            {
                var dto = _mapper.Map<AppEntranceDto>(item);
                result.Add(dto);
                return dto;
            }
            if (item.Uris?.ContainsKey(clientPlatform) == true)
            {
                var dto = _mapper.Map<AppEntranceDto>(item);
                dto.Uri = item.Uris[clientPlatform];
                result.Add(dto);
                return dto;
            }
            return null;
        }

        public async Task<ComponentConfig> GetComponentConfigAsync(Guid entryId)
        {
            var dictAppEntrances = await StateManager.GetOrAddAsync<IReliableDictionary2<Guid, AppEntrance>>(Service.DictionaryName_AppEntrance);
            using (var tx = StateManager.CreateTransaction())
            {
                var itemWrap = await dictAppEntrances.TryGetValueAsync(tx, entryId);
                if (!itemWrap.HasValue) return null;
                return itemWrap.Value.ComponentConfig;
            }
        }

        public async Task<bool> RemoveEntranceAsync(string clientId, Guid entryId)
        {
            if (clientId == null)
                throw new ArgumentNullException(nameof(clientId));
            if (entryId == null)
                throw new ArgumentNullException(nameof(entryId));
            var dictClients = await StateManager.GetOrAddAsync<IReliableDictionary2<string, Client>>(Service.DictionaryName_Client);
            var dictAppEntrances = await StateManager.GetOrAddAsync<IReliableDictionary2<Guid, AppEntrance>>(Service.DictionaryName_AppEntrance);
            using (var tx = StateManager.CreateTransaction())
            {
                var clientWrap = await dictClients.TryGetValueAsync(tx, clientId);
                if (!clientWrap.HasValue) return false;
                var client = clientWrap.Value;
                if (client.AppEntranceIds.Contains(entryId))
                {
                    await dictAppEntrances.TryRemoveAsync(tx, entryId);
                    client.AppEntranceIds.Remove(entryId);
                }
                await dictClients.TryUpdateAsync(tx, clientId, client, clientWrap.Value);
                await tx.CommitAsync();
                return true;
            }
        }

        public async Task<List<AppEntrance>> GetAppEntrancesRawByClientIdAsync(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentException("不能为空", nameof(clientId));

            var dictClients = await StateManager.GetOrAddAsync<IReliableDictionary2<string, Client>>(Service.DictionaryName_Client);
            var dictAppEntrances = await StateManager.GetOrAddAsync<IReliableDictionary2<Guid, AppEntrance>>(Service.DictionaryName_AppEntrance);
            var token = CancellationToken.None;
            using (var tx = StateManager.CreateTransaction())
            {
                var clientWrap = await dictClients.TryGetValueAsync(tx, clientId);
                if (!clientWrap.HasValue)
                    throw new ArgumentOutOfRangeException("没有clientId的数据", clientId);
                var client = clientWrap.Value;
                var lst = await dictAppEntrances.GetAllByIdsAsync(tx, client.AppEntranceIds, token);
                return lst.OrderBy(o => o.Order).ToList();
            }
        }
    }
}
