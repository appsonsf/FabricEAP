using Common.Filters;
using ConfigMgmt.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigMgmt.Controllers
{
    public partial class WorkbenchController
    {
        [CheckToken]
        [AllowAnonymous]
        [HttpPost("initdata/AppEntrance")]
        public async Task<IActionResult> AddAppEntrancesAsync(List<AppEntrance> model)
        {
            try
            {
                var client = Client.Default();
                await _clientAppService.AddOrUpdateClientAsync(client);

                var result = await _workbenchAppService.AddOrUpdateEntrancesAsync(new AddEntrancesInput
                {
                    ClientId = client.Id,
                    AppEntrances = model
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CheckToken]
        [AllowAnonymous]
        [HttpDelete("initdata/AppEntrance")]
        public async Task<IActionResult> RemoveAppEntranceAsync(Guid appId, string clientId = Client.DefaultId)
        {
            try
            {
                if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
                if (appId == Guid.Empty) throw new ArgumentOutOfRangeException(nameof(appId));

                var result = await _workbenchAppService.RemoveEntranceAsync(clientId, appId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CheckToken]
        [AllowAnonymous]
        [HttpPut("initdata/AppEntrance")]
        public async Task<IActionResult> UpdateAppEntranceAsync(AppEntrance model)
        {
            try
            {
                if (model == null) throw new ArgumentNullException(nameof(model));
                if (model.Id == Guid.Empty) throw new ArgumentOutOfRangeException(nameof(model.Id));

                var client = Client.Default();
                var result = await _workbenchAppService.AddOrUpdateEntrancesAsync(new AddEntrancesInput
                {
                    ClientId = client.Id,
                    AppEntrances = new List<AppEntrance> { model }
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CheckToken]
        [AllowAnonymous]
        [HttpGet("initdata/AppEntrance")]
        public async Task<ActionResult<List<AppEntrance>>> GetAppEntrancesAsync(string clientId = Client.DefaultId)
        {
            try
            {
                var result = await _workbenchAppService.GetAppEntrancesRawByClientIdAsync(clientId);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CheckToken]
        [AllowAnonymous]
        [HttpPost("initdata/TodoCenterBizSystem")]
        public async Task<IActionResult> AddOrUpdateTodoCenterBizSystemAsync(TodoCenterBizSystem model)
        {
            try
            {
                if (model == null) throw new ArgumentNullException(nameof(model));
                if (string.IsNullOrEmpty(model.Id)) throw new ArgumentOutOfRangeException(nameof(model.Id));

                return Ok(await _bizSystemAppService.AddOrUpdateForTodoCenterAsync(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CheckToken]
        [AllowAnonymous]
        [HttpDelete("initdata/TodoCenterBizSystem")]
        public async Task<IActionResult> RemoveTodoCenterBizSystemAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

                return Ok(await _bizSystemAppService.RemoveForTodoCenterAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CheckToken]
        [AllowAnonymous]
        [HttpGet("initdata/TodoCenterBizSystem")]
        public async Task<ActionResult<List<TodoCenterBizSystem>>> GetTodoCenterBizSystemsAsync()
        {
            try
            {
                return await _bizSystemAppService.GetAllForTodoCenterAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CheckToken]
        [AllowAnonymous]
        [HttpPost("initdata/MsgNotifyBizSystem")]
        public async Task<IActionResult> AddOrUpdateMsgNotifyBizSystemAsync(MsgNotifyBizSystem model)
        {
            try
            {
                if (model == null) throw new ArgumentNullException(nameof(model));
                if (string.IsNullOrEmpty(model.Id)) throw new ArgumentOutOfRangeException(nameof(model.Id));

                return Ok(await _bizSystemAppService.AddOrUpdateForMsgNotifyAsync(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CheckToken]
        [AllowAnonymous]
        [HttpDelete("initdata/MsgNotifyBizSystem")]
        public async Task<IActionResult> RemoveMsgNotifyBizSystemAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

                return Ok(await _bizSystemAppService.RemoveForMsgNotifyAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CheckToken]
        [AllowAnonymous]
        [HttpGet("initdata/MsgNotifyBizSystem")]
        public async Task<ActionResult<List<MsgNotifyBizSystem>>> GetMsgNotifyBizSystemsAsync()
        {
            try
            {
                return await _bizSystemAppService.GetAllForMsgNotifyAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
