using Common;
using MassTransit;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Notification;
using Notification.MsgContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotifySessionActor
{
    /// <summary>
    /// NotifySessionActor
    /// </summary>
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Volatile)]
    public class TheActor : ActorEapBase, INotifySessionActor
    {
        public bool IsInUnitTest = false;

        public const string ConnectionIdListPropertyName = "ConnectionIdList";
        public const string MessageListPropertyName = "MessageNotifyList";
        public const string EventListPropertyName = "EventNotifyList";

        /// <summary>
        /// Initializes a new instance of NotifySessionActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public TheActor(TheActorService actorService, ActorId actorId)
             : base(actorService, actorId)
        {
        }

        public new TheActorService ActorService => (TheActorService)base.ActorService;

        public async Task AddConnectionIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            try
            {
                var listConnectionId = await StateManager.GetOrAddStateAsync(ConnectionIdListPropertyName, new List<string>());
                if (!listConnectionId.Contains(id))
                    listConnectionId.Add(id);
                await StateManager.SetStateAsync(ConnectionIdListPropertyName, listConnectionId);

                var listMessageNotifyWrap = await StateManager.TryGetStateAsync<List<MessageNotifyDto>>(MessageListPropertyName);
                if (listMessageNotifyWrap.HasValue)
                {
                    await ActorService.EventBus.Send(new SendMessageNotify
                    {
                        UserId = this.GetActorId().GetGuidId(),
                        Notifies = listMessageNotifyWrap.Value
                    });
                    await StateManager.RemoveStateAsync(MessageListPropertyName);
                }

                var listEventNotifyWrap = await StateManager.TryGetStateAsync<List<EventNotifyDto>>(EventListPropertyName);
                if (listEventNotifyWrap.HasValue)
                {
                    await ActorService.EventBus.Send(new SendEventNotify
                    {
                        UserId = this.GetActorId().GetGuidId(),
                        Notifies = listEventNotifyWrap.Value
                    });
                    await StateManager.RemoveStateAsync(EventListPropertyName);
                }
            }
            catch (Exception ex)
            {
                ActorEventSource.Current.Message(ex.ToString());
                if (IsInUnitTest) throw;
            }
        }

        public async Task PushEventNotifyAsync(EventNotifyDto dto)
        {
            var listConnectionIdsWrap = await StateManager.TryGetStateAsync<List<string>>(ConnectionIdListPropertyName);
            if (listConnectionIdsWrap.HasValue && listConnectionIdsWrap.Value.Count > 0)
            {
                await ActorService.EventBus.Send(new SendEventNotify
                {
                    UserId = this.GetActorId().GetGuidId(),
                    Notifies = new List<EventNotifyDto> { dto }
                });
            }
            else
            {
                var listPushNotify = await StateManager.GetOrAddStateAsync(
                    EventListPropertyName, new List<EventNotifyDto>());
                listPushNotify.Add(dto);
                await StateManager.SetStateAsync(EventListPropertyName, listPushNotify);
            }
        }

        public async Task PushEventNotifiesAsync(List<EventNotifyDto> dtos)
        {
            var listConnectionIdsWrap = await StateManager.TryGetStateAsync<List<string>>(ConnectionIdListPropertyName);
            if (listConnectionIdsWrap.HasValue && listConnectionIdsWrap.Value.Count > 0)
            {
                await ActorService.EventBus.Send(new SendEventNotify
                {
                    UserId = this.GetActorId().GetGuidId(),
                    Notifies = dtos
                });
            }
            else
            {
                var listPushNotify = await StateManager.GetOrAddStateAsync(
                    EventListPropertyName, new List<EventNotifyDto>());
                listPushNotify.AddRange(dtos);
                await StateManager.SetStateAsync(EventListPropertyName, listPushNotify);
            }
        }

        public async Task PushMsgNotifyAsync(MessageNotifyDto dto)
        {
            var listConnectionIdsWrap = await StateManager.TryGetStateAsync<List<string>>(ConnectionIdListPropertyName);
            if (listConnectionIdsWrap.HasValue && listConnectionIdsWrap.Value.Count > 0)
            {
                await ActorService.EventBus.Send(new SendMessageNotify
                {
                    UserId = this.GetActorId().GetGuidId(),
                    Notifies = new List<MessageNotifyDto> { dto }
                });
            }
            else
            {
                var listPushNotify = await StateManager.GetOrAddStateAsync(
                    MessageListPropertyName, new List<MessageNotifyDto>());
                listPushNotify.Add(dto);
                await StateManager.SetStateAsync(MessageListPropertyName, listPushNotify);
            }
        }

        public async Task PushMsgNotifiesAsync(List<MessageNotifyDto> dtos)
        {
            var listConnectionIdsWrap = await StateManager.TryGetStateAsync<List<string>>(ConnectionIdListPropertyName);
            if (listConnectionIdsWrap.HasValue && listConnectionIdsWrap.Value.Count > 0)
            {
                await ActorService.EventBus.Send(new SendMessageNotify
                {
                    UserId = this.GetActorId().GetGuidId(),
                    Notifies = dtos
                });
            }
            else
            {
                var listPushNotify = await StateManager.GetOrAddStateAsync(
                    MessageListPropertyName, new List<MessageNotifyDto>());
                listPushNotify.AddRange(dtos);
                await StateManager.SetStateAsync(MessageListPropertyName, listPushNotify);
            }
        }

        public async Task RemoveConnectionIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            try
            {
                var wrap = await StateManager.TryGetStateAsync<List<string>>(ConnectionIdListPropertyName);
                if (wrap.HasValue)
                {
                    var listConnectionIds = wrap.Value;
                    listConnectionIds.Remove(id);
                    if (listConnectionIds.Count == 0)
                        await StateManager.RemoveStateAsync(ConnectionIdListPropertyName);
                    else
                        await StateManager.SetStateAsync(ConnectionIdListPropertyName, listConnectionIds);
                }
            }
            catch (Exception ex)
            {
                ActorEventSource.Current.Message(ex.ToString());
                if (IsInUnitTest) throw;
            }
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            return Task.CompletedTask;
        }
    }
}
