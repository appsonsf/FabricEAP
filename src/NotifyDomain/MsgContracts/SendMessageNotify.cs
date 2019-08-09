using System;
using System.Collections.Generic;
using System.Text;

namespace Notification
{
    public class SendMessageNotify
    {
        public Guid UserId { get; set; }

        public List<MessageNotifyDto> Notifies { get; set; }
    }
}
