using NotifyGatewayWeb.Hubs;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotifyGatewayWeb.EventBus
{
    public class SendMessageNotifyConsumer : IConsumer<SendMessageNotify>
    {
        private readonly IHubContext<NotifyHub, INotifyClient> _hubContext;

        public SendMessageNotifyConsumer(IHubContext<NotifyHub, INotifyClient> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<SendMessageNotify> context)
        {
            try
            {
                await _hubContext.Clients.User(context.Message.UserId.ToString()).ReceiveMessageNotify(context.Message.Notifies);
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.Message($"SendMessageNotifyConsumer Error: UserId: {context.Message.UserId}, Message: {ex.Message}");
            }
        }
    }
}
