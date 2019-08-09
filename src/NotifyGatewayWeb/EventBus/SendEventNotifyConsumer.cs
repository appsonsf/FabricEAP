using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Notification.MsgContracts;
using NotifyGatewayWeb.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotifyGatewayWeb.EventBus
{
    public class SendEventNotifyConsumer : IConsumer<SendEventNotify>
    {
        private readonly IHubContext<NotifyHub, INotifyClient> _hubContext;

        public SendEventNotifyConsumer(IHubContext<NotifyHub, INotifyClient> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<SendEventNotify> context)
        {
            try
            {
                await _hubContext.Clients.User(context.Message.UserId.ToString()).ReceiveEventNotify(context.Message.Notifies);
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.Message($"SendEventNotifyConsumer Error: UserId: {context.Message.UserId}, Message: {ex.Message}");
            }
        }
    }
}
