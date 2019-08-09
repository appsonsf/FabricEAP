using Attachment;
using Attachment.Entities;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;

namespace AttachmentStateService
{
    public class AttachmentAppService : StatefulRemotingService, IAttachmentAppService
    {
        private const string DictName_List = "AttachmentItemList";
        private const string DictName_Index_ContextId = "AttachmentItemList/index/ContextId";

        private async Task<(
            IReliableDictionary2<string, AttachmentItem>,
            IReliableDictionary2<Guid, HashSet<string>>
            )> GetDicts()
        {
            var dictList = await StateManager.GetOrAddAsync<IReliableDictionary2<string, AttachmentItem>>(DictName_List);
            var dictIndexContextId = await StateManager.GetOrAddAsync<IReliableDictionary2<Guid, HashSet<string>>>(DictName_Index_ContextId);

            return (dictList, dictIndexContextId);
        }

        public AttachmentAppService(StatefulServiceContext serviceContext,
            IReliableStateManager stateManager) : base(serviceContext, stateManager)
        {
        }

        public async Task AddOrUpdateAsync(AttachmentItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (string.IsNullOrEmpty(item.Id))
                throw new ArgumentNullException(nameof(item.Id));

            if (item.Size < 0)
                throw new ArgumentNullException(nameof(item.Size));

            if (string.IsNullOrEmpty(item.Location))
                throw new ArgumentNullException(nameof(item.Location));

            var (dictList, dictIndexContextId) = await GetDicts();
            using (var tx = StateManager.CreateTransaction())
            {
                await dictList.SetAsync(tx, item.Id, item);

                HashSet<string> contextMapping;
                var contextMappingWrap = await dictIndexContextId.TryGetValueAsync(tx, item.ContextId);
                if (contextMappingWrap.HasValue)
                    contextMapping = contextMappingWrap.Value;
                else
                    contextMapping = new HashSet<string>();
                if (contextMapping.Add(item.Id))
                    await dictIndexContextId.SetAsync(tx, item.ContextId, contextMapping);

                await tx.CommitAsync();
            }
        }

        public async Task<AttachmentItem> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("IsNullOrEmpty", nameof(id));

            var (dictList, dictIndexContextId) = await GetDicts();
            using (var tx = StateManager.CreateTransaction())
            {
                var wrap = await dictList.TryGetValueAsync(tx, id);
                if (wrap.HasValue) return wrap.Value;
                return null;
            }
        }

        public async Task<RemotingResult> RemoveByContextIdAsync(Guid contextId)
        {
            var (dictList, dictIndexContextId) = await GetDicts();
            using (var tx = StateManager.CreateTransaction())
            {
                var indexWrap = await dictIndexContextId.TryGetValueAsync(tx, contextId);
                if (!indexWrap.HasValue) return RemotingResult.Fail(1);
                foreach (var item in indexWrap.Value)
                {
                    await dictList.TryRemoveAsync(tx, item);
                }
                await dictIndexContextId.TryRemoveAsync(tx, contextId);

                await tx.CommitAsync();
                return RemotingResult.Success();
            }
        }

        public async Task<RemotingResult> UpdateStatusByIdAsync(string id, UploadStatus status)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("IsNullOrEmpty", nameof(id));

            var (dictList, dictIndexContextId) = await GetDicts();
            using (var tx = StateManager.CreateTransaction())
            {
                var wrap = await dictList.TryGetValueAsync(tx, id);
                if (!wrap.HasValue) return RemotingResult.Fail(1);
                if (status < wrap.Value.Status) return RemotingResult.Fail(2);
                var entity = wrap.Value.DeepCopy();
                entity.Status = status;
                if (status == UploadStatus.Uploaded)
                    entity.Uploaded = DateTimeOffset.UtcNow;
                await dictList.TryUpdateAsync(tx, id, entity, wrap.Value);
                await tx.CommitAsync();
                return RemotingResult.Success();
            }
        }
    }
}
