using System;
using System.Collections.Generic;

namespace Base.Eap.Notify.MsgContracts
{
    /// <summary>
    /// 业务系统发送的提醒消息
    /// </summary>
    public class BizSystemNotifyMsg
    {
        public const string RabbitMqReceiveEndpointName = "Base.Eap.BizSystemNotifyService";

        /// <summary>
        /// 发送业务系统
        /// </summary>
        public string SystemId { get; set; }

        /// <summary>
        /// 消息类别，如果设置为0，那么就是使用Content
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 消息类别参数，与消息内容任选其一，如果是使用SMS发送，一般需要设置此属性
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// 接收人的员工号，ReceiverNumbers ReceiverUserIds ReceiverMobiles设置其一，建议使用ReceiverUserIds
        /// </summary>
        public string[] ReceiverNumbers { get; set; }

        /// <summary>
        /// 接收人的UserId，ReceiverNumbers ReceiverUserIds ReceiverMobiles设置其一，建议使用ReceiverUserIds
        /// </summary>
        public Guid[] ReceiverUserIds { get; set; }

        /// <summary>
        /// 接收人的手机号，ReceiverNumbers ReceiverUserIds ReceiverMobiles设置其一，建议使用ReceiverUserIds
        /// </summary>
        [Obsolete("未来手机号不应该由业务系统提供，应该提供UserId或者员工号")]
        public string[] ReceiverMobiles { get; set; }
    }
}
