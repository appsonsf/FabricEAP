using Common;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConfigMgmt.Controllers
{
    [Route("api/v1/usersetting")]
    public class UserSettingController: EapBaseController
    {
        private readonly Func<Guid, IUserSettingAppService> _userSettingAppServiceFactory;
        private readonly ILogger _logger;

        public UserSettingController(Func<Guid,IUserSettingAppService> userSettingAppServiceFactory)
        {
            _userSettingAppServiceFactory = userSettingAppServiceFactory;
            _logger = Log.ForContext<UserSettingController>();
        }

        [HttpPost("infoVisibility")]
        public async Task<ActionResult<ResponseData>> SetInfoVisibility(InfoVisibility model)
        {
            try
            {
                var userId = GetUserId();
                await _userSettingAppServiceFactory(userId).SetInfoVisibilityAsync(userId, model);
                return BuildSuccess();
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }


        [HttpGet("infoVisibility")]
        public async Task<ActionResult<InfoVisibility>> GetInfoVisibility()
        {
            try
            {
                var userId = GetUserId();
                var dto = await _userSettingAppServiceFactory(userId).GetInfoVisibilityAsync(userId);
                return dto;
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }
    }
}
