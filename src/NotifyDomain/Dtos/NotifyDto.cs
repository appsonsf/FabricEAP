using System;
using System.Runtime.Serialization;

namespace Notification
{
    [DataContract]
    public abstract class NotifyDto
    {
        [DataMember]
        public NotifyTargetType Target { get; set; }

        /// <summary>
        /// Target=Client，TargetId是客户端Id
        /// Target=BizSystem，TargetId是NotifyBizSystem的Id
        /// Target=Conversation，TargetId是Conversation的Id
        /// </summary>
        [DataMember]
        public string TargetId { get; set; }

        [DataMember]
        public int TargetCategory { get; set; }
    }
}
