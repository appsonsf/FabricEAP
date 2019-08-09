using System;

namespace InstantMessage.ViewModels
{
    public class MessageVmBase
    {
        /// <summary>
        /// 发送者Id
        /// </summary>
        public Guid SenderId { get; set; }
        /// <summary>
        /// 会话Id
        /// </summary>
        public Guid ConversationId { get; set; }

        /// <summary>
        /// 会话类型
        /// </summary>
        public ConversationType ConversationType { get; set; }
    }
}
