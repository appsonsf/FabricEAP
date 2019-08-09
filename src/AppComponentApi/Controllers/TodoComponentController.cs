using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AppComponent.ViewModels;
using ConfigMgmt;
using ConfigMgmt.Entities;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace AppComponent.Controllers
{
    [Route("api/v1/appcom/todo")]
    public class TodoComponentController : EapBaseController
    {
        private readonly ITodoApiClient _todoApiClient;
        private readonly IWorkbenchAppService _workbenchAppService;
        private readonly ILogger _logger;

        public TodoComponentController(ITodoApiClient todoApiClient, IWorkbenchAppService workbenchAppService)
        {
            _todoApiClient = todoApiClient;
            _workbenchAppService = workbenchAppService;
            _logger = Log.ForContext<TodoComponentController>();
        }

        [Route("Pending")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<TodoListVM>>> GetPendingAsync(Guid appId)
        {
            try
            {
                var config = await _workbenchAppService.GetComponentConfigAsync(appId);
                if (config == null)
                    return NotFound(new { message = "componentConfigId Not Found" });
                var list = await _todoApiClient.GetPendingListAsync(config);
                return list;
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        [Route("Done")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<TodoListVM>>> GetDoneAsync(Guid appId)
        {
            try
            {
                var config = await this._workbenchAppService.GetComponentConfigAsync(appId);
                if (config == null)
                    return NotFound(new { message = "ComponentConfigId Not Found" });
                var list = await _todoApiClient.GetDoneListAsync(config);
                return list;
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }
    }
}
