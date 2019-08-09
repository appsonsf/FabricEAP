using Common;
using Common.Services;
using ConfigMgmt.Entities;
using ConfigMgmt.ViewModels;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigMgmt.Controllers
{
    [Route("api/v1/workbench")]
    public partial class WorkbenchController : EapBaseController
    {
        private readonly IWorkbenchAppService _workbenchAppService;
        private readonly IBadgeApiClient _badgeApiClient;
        private readonly IClientAppService _clientAppService;
        private readonly IBizSystemAppService _bizSystemAppService;
        private readonly IMobileCodeSender _mobileCodeSender;
        private readonly Func<Guid, IUserSettingAppService> _userSettingAppServiceFactory;
        private readonly ILogger _logger;

        public WorkbenchController(IWorkbenchAppService workbenchAppService,
            IClientAppService clientAppService,
            IBizSystemAppService bizSystemAppService,
            IBadgeApiClient badgeApiClient,
            IMobileCodeSender mobileCodeSender,
            Func<Guid, IUserSettingAppService> userSettingAppServiceFactory)
        {
            _workbenchAppService = workbenchAppService;
            _badgeApiClient = badgeApiClient;
            _clientAppService = clientAppService;
            _bizSystemAppService = bizSystemAppService;
            _mobileCodeSender = mobileCodeSender;
            _userSettingAppServiceFactory = userSettingAppServiceFactory;
            _logger = Log.ForContext<WorkbenchController>();
        }

        [HttpGet]
        public async Task<ActionResult<List<AppEntranceDto>>> GetAll(ClientPlatform clientPlatform, string clientId = Client.DefaultId)
        {
            try
            {
                var input = new GetAppEntrancesInput
                {
                    ClientId = clientId,
                    UserRoles = GetUserRoles().ToList(),
                    ClientPlatform = clientPlatform,
                };

                return await _workbenchAppService.GetAppEntrancesAsync(input);
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        [HttpGet("badge")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> GetBadgeAmount(Guid entryId)
        {
            try
            {
                var config = await _workbenchAppService.GetComponentConfigAsync(entryId);

                if (config == null || config.DataSources == null)
                    return 0;

                var result = await _badgeApiClient.GetAmountAsync(config);
                if (result == null)
                    return 0;

                return result.Value;
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <returns>Data=手机号后4位, Status=1 手机号不存在 </returns>
        [HttpGet("authcode")]
        public async Task<ActionResult<ResponseData<string>>> SendAuthCode()
        {
            try
            {
                var mobile = GetUserMobile();
                if (string.IsNullOrEmpty(mobile)) return BuildFaild<string>(1);

                var code = await _mobileCodeSender.SendAsync(mobile);

#if DEBUG
                return BuildSuccess(mobile.Substring(mobile.Length - 4, 4) + ":" + code);
#else
                return BuildSuccess(mobile.Substring(mobile.Length - 4, 4));
#endif
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        /// <summary>
        /// 验证手机验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true=验证成功</returns>
        [HttpPost("authcode")]
        public async Task<ActionResult<bool>> CheckAuthCode(CheckAuthCodeVm model)
        {
            try
            {
                if (model == null)
                    throw new ArgumentNullException(nameof(model));

                var mobile = GetUserMobile();
                if (string.IsNullOrEmpty(mobile)) return false;

                var verified = await _mobileCodeSender.CheckAsync(mobile, model.MobileCode);
                if (!verified) return false;

                var userId = GetUserId();
                var userSettingAppService = _userSettingAppServiceFactory(userId);
                await userSettingAppService.SetAppEntranceAuthStateAsync(userId, new AppEntranceAuthStateInput
                {
                    AppEntranceId = model.AppEntranceId,
                    DeviceCode = model.DeviceCode
                });

                return true;
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        /// <summary>
        /// 检查用户的验证状态
        /// </summary>
        /// <param name="appEntranceId"></param>
        /// <param name="deviceCode"></param>
        /// <returns>true验证无效需要验证，false验证有效不需要验证</returns>
        [HttpGet("authstatus")]
        public async Task<ActionResult<bool>> CheckAuthStatus(Guid appEntranceId, string deviceCode)
        {
            try
            {
                if (appEntranceId == Guid.Empty) throw new ArgumentOutOfRangeException(nameof(appEntranceId));
                if (string.IsNullOrEmpty(deviceCode)) throw new ArgumentNullException(nameof(deviceCode));

                var userId = GetUserId();
                var userSettingAppService = _userSettingAppServiceFactory(userId);
                return await userSettingAppService.GetAppEntranceAuthStateAsync(userId, deviceCode, appEntranceId);
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }
    }
}
