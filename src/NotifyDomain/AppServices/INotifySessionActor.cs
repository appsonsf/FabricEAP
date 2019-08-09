using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Notification
{
    /// <summary>
    /// 代表用户Session的Actor
    /// </summary>
    public interface INotifySessionActor : IActor
    {
        Task PushMsgNotifyAsync(MessageNotifyDto dto);

        Task PushMsgNotifiesAsync(List<MessageNotifyDto> dtos);

        Task PushEventNotifyAsync(EventNotifyDto dto);

        Task PushEventNotifiesAsync(List<EventNotifyDto> dtos);

        Task AddConnectionIdAsync(string id);

        Task RemoveConnectionIdAsync(string id);
    }
}
