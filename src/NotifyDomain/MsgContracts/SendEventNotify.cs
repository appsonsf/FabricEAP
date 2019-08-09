using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.MsgContracts
{
    public class SendEventNotify
    {
        public Guid UserId { get; set; }

        public List<EventNotifyDto> Notifies { get; set; }
    }
}
