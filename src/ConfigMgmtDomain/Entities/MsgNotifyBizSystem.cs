using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConfigMgmt.Entities
{
    /// <summary>
    /// 可以发送消息提醒的业务系统
    /// </summary>
    [DataContract]
    public class MsgNotifyBizSystem
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string AppId { get; set; }

        /// <summary>
        /// 发送提醒的方式，可以多个
        /// </summary>
        [DataMember]
        public MsgNotifyApproach[] Approachs { get; set; }

        /// <summary>
        /// 发送提醒的模式，所有方式或者按顺序选择一个
        /// </summary>
        [DataMember]
        public bool UseAllApproachs { get; set; }

        /// <summary>
        /// 不同消息类别所具有的内容Pattern
        /// </summary>
        [DataMember]
        public Dictionary<int, string> PatternOfCategory { get; set; }
    }
}
