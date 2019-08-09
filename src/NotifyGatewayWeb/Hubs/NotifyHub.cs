using Notification;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace NotifyGatewayWeb.Hubs
{
    [Authorize]
    public class NotifyHub : Hub<INotifyClient>
    {
        public override async Task OnConnectedAsync()
        {
            try
            {
                await RemotingClient.CreateNotifySessionActor(GetUserId())
                    .AddConnectionIdAsync(Context.ConnectionId);
            }
            catch (Exception e)
            {
                Debug.WriteLine("OnConnectedAsync Exception === " + e.Message);
            }
            await base.OnConnectedAsync();
        }

        private Guid GetUserId()
        {
            var claim = Context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null) return Guid.Empty;
            return claim.Value.ToGuid();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                await RemotingClient.CreateNotifySessionActor(GetUserId())
                    .RemoveConnectionIdAsync(Context.ConnectionId);
            }
            catch (Exception e)
            {
                Debug.WriteLine("OnDisconnectedAsync Exception === " + e.Message);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
