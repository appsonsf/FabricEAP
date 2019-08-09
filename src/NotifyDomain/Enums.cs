using System;
using System.Collections.Generic;
using System.Text;

namespace Notification
{
    public enum NotifyTargetType
    {
        /// <summary>
        /// 针对整个客户端
        /// </summary>
        Client = 1,
        /// <summary>
        /// 针对业务系统
        /// </summary>
        BizSystem = 2,
        /// <summary>
        /// 针对会话
        /// </summary>
        Conversation = 3,
    }
}
