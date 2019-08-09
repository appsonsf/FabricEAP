using ConfigMgmt;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading.Tasks;

namespace UserConfigStateService
{
    public class UserFavoriteAppService : StatefulRemotingService, IUserFavoriteAppService
    {
        private const string DictionaryName = "EmployeeFavs";

        public UserFavoriteAppService(StatefulServiceContext serviceContext, IReliableStateManager stateManager)
            : base(serviceContext, stateManager)
        {

        }

        public async Task<bool> AddEmployeeAsync(Guid userId, Guid employeeId)
        {
            var dictionary = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, HashSet<Guid>>>(DictionaryName);
            using (var tx = StateManager.CreateTransaction())
            {
                var wrap = await dictionary.TryGetValueAsync(tx, userId);
                if (wrap.HasValue)
                {
                    var oldValue = new HashSet<Guid>(wrap.Value);
                    var result = wrap.Value.Add(employeeId);
                    if (result)
                    {
                        await dictionary.TryUpdateAsync(tx, userId, wrap.Value, oldValue);
                        await tx.CommitAsync();
                    }
                    return true;
                }
                else
                {
                    await dictionary.AddAsync(tx, userId, new HashSet<Guid>(new[] { employeeId }));
                    await tx.CommitAsync();
                    return true;
                }
            }
        }

        public async Task<bool> RemoveEmployeeAsync(Guid userId, Guid employeeId)
        {
            var dictionary = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, HashSet<Guid>>>(DictionaryName);
            using (var tx = StateManager.CreateTransaction())
            {
                if (await dictionary.ContainsKeyAsync(tx, userId))
                {
                    var wrap = await dictionary.TryGetValueAsync(tx, userId);
                    if (wrap.HasValue)
                    {
                        var oldValue = new HashSet<Guid>(wrap.Value);
                        var employeeIds = wrap.Value;
                        if (!employeeIds.Contains(employeeId)) return true;
                        employeeIds.Remove(employeeId);
                        if (employeeIds.Count == 0)
                        {
                            var tryRemoveResult = await dictionary.TryRemoveAsync(tx, userId);
                            await tx.CommitAsync();
                            return true;
                        }
                        await dictionary.SetAsync(tx, userId, employeeIds);
                        await tx.CommitAsync();
                        return true;
                    }
                    return true;
                }
                return true;
            }
        }

        public async Task<Guid[]> GetEmployeesAsync(Guid userId)
        {
            var dictionary = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, HashSet<Guid>>>(DictionaryName);
            using (var tx = StateManager.CreateTransaction())
            {
                var wrap = await dictionary.TryGetValueAsync(tx, userId);
                if (wrap.HasValue)
                {
                    return wrap.Value.ToArray();
                }
                return new Guid[0];
            }
        }

        public async Task<bool> IsFavoritedAsync(Guid userId, Guid employeeId)
        {
            var favs = await GetEmployeesAsync(userId);
            return favs.Contains(employeeId);
        }
    }
}
