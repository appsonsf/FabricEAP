using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigMgmt;
using ConfigMgmt.Entities;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Nest;
using ServiceFabricContrib;

namespace UserConfigStateService
{
    public class UserSettingAppService : StatefulRemotingService, IUserSettingAppService
    {
        private const string DictionaryName_InfoVisibility = "UserSetttings_InfoVisibility";
        private const string DictionaryName_UserDeviceBinding = "UserSettings/UserDeviceBinding";

        public UserSettingAppService(StatefulServiceContext serviceContext, IReliableStateManager stateManager) : base(serviceContext, stateManager)
        {
        }

        private static readonly TimeSpan AppEntranceAuthStateValidity = TimeSpan.FromDays(1);

        public async Task<bool> GetAppEntranceAuthStateAsync(Guid userId, string deviceCode, Guid appEntranceId)
        {
            var dictionary = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, UserDeviceBinding>>(DictionaryName_UserDeviceBinding);
            using (var tx = StateManager.CreateTransaction())
            {
                var wrap = await dictionary.TryGetValueAsync(tx, userId);
                if (wrap.HasValue)
                {
                    var entity = wrap.Value;
                    return entity.CheckWhetherAuth(deviceCode, appEntranceId, AppEntranceAuthStateValidity);
                }
                else
                    return true;
            }
        }

        public async Task<InfoVisibility> GetInfoVisibilityAsync(Guid userId)
        {
            var dictionary = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, InfoVisibility>>(DictionaryName_InfoVisibility);
            using (var tx = StateManager.CreateTransaction())
            {
                var rDic = await dictionary.TryGetValueAsync(tx, userId);
                if (rDic.HasValue)
                {
                    return rDic.Value;
                }

                return new InfoVisibility();
            }
        }

        public async Task SetAppEntranceAuthStateAsync(Guid userId, AppEntranceAuthStateInput input)
        {
            var dictionary = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, UserDeviceBinding>>(DictionaryName_UserDeviceBinding);
            using (var tx = StateManager.CreateTransaction())
            {
                UserDeviceBinding entity;
                var wrap = await dictionary.TryGetValueAsync(tx, userId);
                if (wrap.HasValue)
                {
                    entity = wrap.Value;
                    if (entity.Id == Guid.Empty) entity.Id = userId;//fix existing data for DataMember missing bug #1657
                    entity.AddOrUpdateLatestAuthed(input.DeviceCode, input.AppEntranceId);
                    await dictionary.SetAsync(tx, userId, entity);
                }
                else
                {
                    entity = UserDeviceBinding.Create(userId, input.DeviceCode, input.AppEntranceId);
                    await dictionary.AddAsync(tx, userId, entity);
                }
                await tx.CommitAsync();
            }
        }

        public async Task SetInfoVisibilityAsync(Guid userId, InfoVisibility value)
        {
            var dictionary = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, InfoVisibility>>(DictionaryName_InfoVisibility);
            using (var tx = StateManager.CreateTransaction())
            {
                await dictionary.SetAsync(tx, userId, value);
                await tx.CommitAsync();
            }
        }
    }
}
