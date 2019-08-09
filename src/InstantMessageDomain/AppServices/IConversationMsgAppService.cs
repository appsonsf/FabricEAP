using InstantMessage.Entities;
using Microsoft.ServiceFabric.Services.Remoting;
using Notification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InstantMessage
{
    public interface IConversationMsgAppService : IService
    {
        Task SendMessageAsync(ConversationMsg msg);

        Task<List<ConversationMsg>> GetMessagesAsync(List<MessageNotifyDto> input);

        /// <summary>
        /// 归档群组消息
        /// </summary>
        /// <param name="id">群组Id</param>
        /// <returns></returns>
        Task ArchiveGroupMessagesAsync(Guid id);

        Task<List<ConversationMsg>> GetOldMessagesAsync(GetOldMessagesInput input);
    }
}
