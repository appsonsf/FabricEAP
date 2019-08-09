using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Notification
{
    [DataContract]
    public class MessageNotifyDto : NotifyDto
    {
        [DataMember]
        public Guid LatestMsgId { get; set; }
    }
}
