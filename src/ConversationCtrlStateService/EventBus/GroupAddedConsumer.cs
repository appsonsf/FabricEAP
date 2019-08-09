﻿using ConversationCtrlStateService.DomainServices;
using EnterpriseContact;
using EnterpriseContact.MsgConstracts;
using InstantMessage;
using InstantMessage.Entities;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using Microsoft.ServiceFabric.Data;
using Nest;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConversationCtrlStateService.EventBus
{
    public class GroupAddedConsumer : IConsumer<GroupAdded>
    {
        private readonly IGroupAppService _groupAppService;
        private readonly IReliableStateManager _stateManager;
        private readonly ConversationManager _manager;
        private readonly ILogger _logger;

        public GroupAddedConsumer(IReliableStateManager stateManager, IGroupAppService groupAppService)
        {
            _groupAppService = groupAppService;
            _stateManager = stateManager;
            _manager = new ConversationManager(stateManager);
            _logger = Log.ForContext<GroupAddedConsumer>();
        }

        public async Task Consume(ConsumeContext<GroupAdded> context)
        {
            try
            {
                var participants = await _groupAppService.GetUserIdsByIdAsync(context.Message.Id);
                await _manager.AddOrUpdateAsync(context.Message.Id, participants, ConversationType.CustomGroup);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                throw;
            }
        }
    }
}
