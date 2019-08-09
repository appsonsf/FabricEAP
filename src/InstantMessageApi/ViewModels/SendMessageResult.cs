using System;
using System.Collections.Generic;
using System.Text;

namespace InstantMessage.ViewModels
{
    /// <summary>
    /// 消息发送的结果
    /// </summary>
    public class SendMessageResult
    {
        /// <summary>
        /// 标识发送是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 原因补充
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 消息Id
        /// </summary>
        public Guid MessageId { get; set; }
        /// <summary>
        /// 消息发送时间
        /// </summary>
        public DateTimeOffset SendTime { get; set; }
    }
}
