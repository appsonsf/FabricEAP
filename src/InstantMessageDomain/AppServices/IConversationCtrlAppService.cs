using InstantMessage.Entities;
using Microsoft.ServiceFabric.Services.Remoting;
using Notification;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstantMessage
{
    public interface IConversationCtrlAppService : IService
    {
        Task<Conversation> GetOrAddP2PAsync(Guid senderId, Guid receiverId);

        Task<Conversation?> GetByIdAsync(Guid id);

        /// <summary>
        /// 标记会话为删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// 添加部门群组会话
        /// </summary>
        /// <param name="input"></param>
        /// <returns>FailedCode 1:NotInDepartment 2:OnlyOneMember</returns>
        Task<RemotingResult> AddDepAsync(AddDepConversationInput input);

        /// <summary>
        /// 基于Id集合得到会话
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<List<Conversation>> GetByIdsAsync(List<Guid> ids);
    }
}
