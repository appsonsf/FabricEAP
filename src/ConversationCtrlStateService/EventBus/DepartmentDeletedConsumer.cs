using ConversationCtrlStateService.DomainServices;
using EnterpriseContact.MsgConstracts;
using MassTransit;
using Microsoft.ServiceFabric.Data;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConversationCtrlStateService.EventBus
{
    public class DepartmentDeletedConsumer : IConsumer<DepartmentDeleted>
    {
        private readonly IReliableStateManager _stateManager;
        private readonly ILogger _logger;
        private readonly ConversationManager _manager;

        public DepartmentDeletedConsumer(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
            _logger = Log.ForContext<DepartmentDeletedConsumer>();
            _manager = new ConversationManager(stateManager);
        }

        public async Task Consume(ConsumeContext<DepartmentDeleted> context)
        {
            try
            {
                await _manager.DeleteAsync(context.Message.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                throw;
            }
        }
    }
}
