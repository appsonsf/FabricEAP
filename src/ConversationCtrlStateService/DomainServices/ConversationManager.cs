using InstantMessage;
using InstantMessage.Entities;
using Microsoft.ServiceFabric.Data;
using Notification;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConversationCtrlStateService.DomainServices
{
    public class ConversationManager
    {
        private readonly IReliableStateManager _stateManager;

        public ConversationManager(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task AddOrUpdateAsync(Guid id, List<Guid> participants, ConversationType type)
        {
            var dictList = await Service.GetListDictAsync(_stateManager);
            using (var tx = _stateManager.CreateTransaction())
            {
                var newEntity = new Conversation
                {
                    Id = id,
                    Type = type,
                    Participants = participants,
                    Updated = DateTimeOffset.UtcNow
                };
                await dictList.AddOrUpdateAsync(tx, id, newEntity, (k, v) => newEntity);
                await tx.CommitAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var dictKeyToId = await Service.GetKeyToIdDictAsync(_stateManager);
            var dictList = await Service.GetListDictAsync(_stateManager);
            using (var tx = _stateManager.CreateTransaction())
            {
                var entityWrap = await dictList.TryGetValueAsync(tx, id);
                if (entityWrap.HasValue)
                {
                    var edited = entityWrap.Value.DeepCopy();
                    edited.IsDeleted = true;
                    await dictList.TryUpdateAsync(tx, id, edited, entityWrap.Value);

                    if (entityWrap.Value.Type == ConversationType.P2P && !string.IsNullOrEmpty(entityWrap.Value.Key))
                    {
                        await dictKeyToId.TryRemoveAsync(tx, entityWrap.Value.Key);
                    }

                    await tx.CommitAsync();
                }
            }
        }

        public async Task<Conversation?> GetByIdAsync(Guid id)
        {
            var dictIdToEntity = await Service.GetListDictAsync(_stateManager);
            using (var tx = _stateManager.CreateTransaction())
            {
                var entityWrap = await dictIdToEntity.TryGetValueAsync(tx, id);
                if (entityWrap.HasValue)
                    return entityWrap.Value;
                return null;
            }
        }
    }
}
