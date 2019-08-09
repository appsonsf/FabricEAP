using Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotifyGatewayWeb.Hubs
{
    public interface INotifyClient
    {
        Task ReceiveEventNotify(List<EventNotifyDto> notifies);
        Task ReceiveMessageNotify(List<MessageNotifyDto> notifies);
    }
}
