using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Common
{
    [Authorize]
    [ApiController]
    public abstract class EapBaseController : ControllerBase
    {
        protected ResponseData<T> BuildSuccess<T>(T data)
        {
            return ResponseData<T>.BuildSuccessResponse(data);
        }

        protected ResponseData BuildSuccess()
        {
            return ResponseData.BuildSuccessResponse();
        }

        protected ResponseData<T> BuildFaild<T>()
        {
            return ResponseData<T>.BuildFailedResponse(StatusConst.DefaultFailureStatus);
        }

        protected ResponseData<T> BuildFaild<T>(string message)
        {
            return ResponseData<T>.BuildFailedResponse(StatusConst.DefaultFailureStatus, message);
        }

        protected ResponseData<T> BuildFaild<T>(int code)
        {
            return ResponseData<T>.BuildFailedResponse(code);
        }

        protected ResponseData<T> BuildFaild<T>(int code, string message)
        {
            return ResponseData<T>.BuildFailedResponse(code, message);
        }

        protected ResponseData BuildFaild()
        {
            return ResponseData.BuildFailedResponse(StatusConst.DefaultFailureStatus);
        }

        protected ResponseData BuildFaild(string message)
        {
            return ResponseData.BuildFailedResponse(StatusConst.DefaultFailureStatus, message);
        }

        protected ResponseData BuildFaild(int code)
        {
            return ResponseData.BuildFailedResponse(code);
        }

        protected ResponseData BuildFaild(int code, string message)
        {
            return ResponseData.BuildFailedResponse(code, message);
        }

        /// <summary>
        /// 获取当前用户的名称
        /// </summary>
        /// <returns></returns>
        protected string GetUserName()
        {
            if (!User.Identity.IsAuthenticated)
                return string.Empty;
            var claim = User.FindFirst("preferred_username");
            if (claim == null)
                return string.Empty;
            return claim.Value;
        }

        /// <summary>
        /// 获取当前用户的名称
        /// </summary>
        /// <returns></returns>
        protected string GetUserFullName()
        {
            if (!User.Identity.IsAuthenticated)
                return string.Empty;
            var claim = User.FindFirst("name");
            if (claim == null)
                return string.Empty;
            return claim.Value;
        }

        /// <summary>
        /// 获取当前用户的UserId
        /// </summary>
        /// <returns></returns>
        protected Guid GetUserId()
        {
            if (!User.Identity.IsAuthenticated)
                return Guid.Empty;
            //var claim = User.Claims.FirstOrDefault(u => u.Type == "sub");
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null || string.IsNullOrEmpty(claim.Value))
                return Guid.Empty;
            return new Guid(claim.Value);
        }

        /// <summary>
        /// 获取当前用户的手机号
        /// </summary>
        /// <returns></returns>
        protected string GetUserMobile()
        {
            if (!User.Identity.IsAuthenticated)
                return string.Empty;
            var claim = User.Claims.FirstOrDefault(u => u.Type == "phone_number");
            if (claim == null)
                return string.Empty;
            return claim.Value;
        }

        /// <summary>
        /// 获取当前用户的员工号
        /// </summary>
        /// <returns></returns>
        protected string GetEmployeeNumber()
        {
            if (!User.Identity.IsAuthenticated)
                return string.Empty;
            var claim = User.FindFirst("employee_number");
            if (claim == null)
                return string.Empty;
            return claim.Value;
        }

        /// <summary>
        /// 获取当前用户的员工号
        /// </summary>
        /// <returns></returns>
        protected Guid GetEmployeeId()
        {
            if (!User.Identity.IsAuthenticated)
                return Guid.Empty;
            var claim = User.FindFirst("employee_mdmid");
            if (claim == null || string.IsNullOrEmpty(claim.Value))
                return Guid.Empty;
            return new Guid(claim.Value);
        }

        /// <summary>
        /// 获取当前用户的身份证号
        /// </summary>
        /// <returns></returns>
        protected string GetIdCardNo()
        {
            if (!User.Identity.IsAuthenticated)
                return string.Empty;
            var claim = User.FindFirst("idcard_number");
            if (claim == null)
                return string.Empty;
            return claim.Value;
        }

        /// <summary>
        /// 获取当前用户的角色数组
        /// </summary>
        /// <returns></returns>
        protected string[] GetUserRoles()
        {
            if (!User.Identity.IsAuthenticated)
                return new string[0];
            var claim = User.FindFirst(ClaimTypes.Role);
            if (claim == null)
                return new string[0];
            return claim.Value.Split(',');
        }

        protected Dictionary<string, string> GetUserContext()
        {
            try
            {
                return new Dictionary<string, string>
                {
                    ["UserName"] = GetUserName(),
                    ["UserFullName"] = GetUserFullName(),
                    ["UserId"] = GetUserId().ToString(),
                    ["UserMobile"] = GetUserMobile(),
                    ["EmployeeNumber"] = GetEmployeeNumber(),
                    ["EmployeeId"] = GetEmployeeId().ToString(),
                    ["IdCardNo"] = GetIdCardNo(),
                    ["UserRoles"] = string.Join(",", GetUserRoles())
                };
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }

        protected string LogError(ILogger logger, Exception ex)
        {
            logger.Error(ex, "Api Error. \r\n" +
                "UserContext:{@UserContext}. \r\n" +
                "RequestPath: {RequestPath}. \r\n" +
                "RequestQuery: {@RequestQuery}. ",
                GetUserContext(),
                Request.Path.Value,
                Request.Query);

            return ex.Message;
        }
    }
}
