using ConfigMgmt;
using ConfigMgmt.Entities;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using ServiceFabricContrib;
using System;
using System.Fabric;
using System.Threading.Tasks;

namespace SystemConfigStateService
{
    public class ClientAppService : StatefulRemotingService, IClientAppService
    {
        public ClientAppService(StatefulServiceContext serviceContext, IReliableStateManager stateManager)
           : base(serviceContext, stateManager)
        {

        }

        public async Task<bool> AddOrUpdateClientAsync(Client input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (string.IsNullOrEmpty(input.Id))
                throw new ArgumentException("ClientId不能为空", nameof(input.Id));

            var dictionary = await StateManager.GetOrAddAsync<IReliableDictionary2<string, Client>>(Service.DictionaryName_Client);
            using (var tx = StateManager.CreateTransaction())
            {
                await dictionary.AddOrUpdateAsync(tx, input.Id, input, (k, o) => input);

                await tx.CommitAsync();
                return true;
            }
        }
    }
}
