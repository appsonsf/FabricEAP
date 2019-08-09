using Attachment.Entities;
using Microsoft.ServiceFabric.Services.Remoting;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Attachment
{
    public interface IAttachmentAppService : IService
    {
        /// <summary>
        /// 基于文件的md5值得到现有的附件数据项
        /// </summary>
        /// <param name="id">md5 hash value</param>
        /// <returns></returns>
        Task<AttachmentItem> GetByIdAsync(string id);

        /// <summary>
        /// 添加附件数据项
        /// </summary>
        /// <param name="item"></param>
        /// <returns>
        /// </returns>
        Task AddOrUpdateAsync(AttachmentItem item);
        /// <summary>
        /// 基于ContextId删除附件数据项
        /// </summary>
        /// <param name="contextId"></param>
        /// <returns>
        /// FailedCode=1：contextId不存在；
        /// </returns>
        Task<RemotingResult> RemoveByContextIdAsync(Guid contextId);
        /// <summary>
        /// 更新附件数据项的状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns>
        /// FailedCode=1: 数据项不存在
        /// FailedCode=2: 状态值不能降级
        /// </returns>
        Task<RemotingResult> UpdateStatusByIdAsync(string id, UploadStatus status);
    }
}
