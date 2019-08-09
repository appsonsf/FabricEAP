using ConversationCtrlStateService.DomainServices;
using EnterpriseContact;
using EnterpriseContact.MsgConstracts;
using InstantMessage;
using MassTransit;
using Microsoft.ServiceFabric.Data;
using Serilog;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConversationCtrlStateService.EventBus
{
    public class DepartmentMembersUpdatedConsumer : IConsumer<DepartmentMembersUpdated>
    {
        private readonly IReliableStateManager _stateManager;
        private readonly ConversationManager _manager;
        private readonly ILogger _logger;
        private readonly IEmployeeAppService _employeeAppService;

        public DepartmentMembersUpdatedConsumer(IReliableStateManager stateManager, IEmployeeAppService employeeAppService)
        {
            _stateManager = stateManager;
            _manager = new ConversationManager(stateManager);
            _logger = Log.ForContext<DepartmentMembersUpdatedConsumer>();
            _employeeAppService = employeeAppService;
        }

        public async Task Consume(ConsumeContext<DepartmentMembersUpdated> context)
        {
            try
            {
                var dictList = await Service.GetListDictAsync(_stateManager);
                using (var tx = _stateManager.CreateTransaction())
                {
                    var entityWrap = await dictList.TryGetValueAsync(tx, context.Message.Id);
                    if (entityWrap.HasValue)
                    {
                        var participants = await _employeeAppService.GetUserIdsByDepartmentIdAsync(context.Message.Id);

                        var edited = entityWrap.Value.DeepCopy();
                        edited.Participants = participants;
                        await dictList.TryUpdateAsync(tx, context.Message.Id, edited, entityWrap.Value);
                    }
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
