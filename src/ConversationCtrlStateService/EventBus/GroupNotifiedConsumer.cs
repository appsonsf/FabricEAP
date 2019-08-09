using ConversationCtrlStateService.DomainServices;
using EnterpriseContact.MsgConstracts;
using MassTransit;
using Microsoft.ServiceFabric.Data;
using Notification;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConversationCtrlStateService.EventBus
{
    public class GroupNotifiedConsumer : IConsumer<GroupNotified>
    {
        private readonly IReliableStateManager _stateManager;
        private readonly ILogger _logger;
        private readonly Func<Guid, INotifySessionActor> _notifySessionActorFactory;
        private readonly ConversationManager _manager;

        public GroupNotifiedConsumer(IReliableStateManager stateManager, 
            Func<Guid, INotifySessionActor> notifySessionActorFactory)
        {
            _stateManager = stateManager;
            _notifySessionActorFactory = notifySessionActorFactory;
            _manager = new ConversationManager(stateManager);
            _logger = Log.ForContext<GroupNotifiedConsumer>();
        }

        public async Task Consume(ConsumeContext<GroupNotified> context)
        {
            try
            {
                var con = await _manager.GetByIdAsync(context.Message.Id);
                if (con.HasValue)
                {
                    var tasks = new List<Task>();
                    foreach (var userId in con.Value.Participants)
                    {
                        var notifySessionActor = _notifySessionActorFactory(userId);
                        tasks.Add(notifySessionActor.PushEventNotifiesAsync(context.Message.Notifies));
                    }
                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                throw;
            }
        }
    }
}
