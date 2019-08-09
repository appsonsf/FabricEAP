using System;
using System.Collections.Generic;
using System.Text;

namespace InstantMessage
{
    [Serializable]
    public struct GetOldMessagesInput
    {
        /// <summary>
        /// ConversationId
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 客户端本地消息中最老的一条消息的Id
        /// 返回的时候不会包含这条消息
        /// </summary>
        public Guid OldestMsgId { get; set; }
    }
}
