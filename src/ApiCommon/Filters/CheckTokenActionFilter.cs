using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Common.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CheckTokenAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //nothing
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Headers.ContainsKey("token"))
            {
                var token = context.HttpContext.Request.Headers["token"];
                if (Guid.TryParse(token, out Guid tokenGuid)
                    && tokenGuid == new Guid("0A8E3BE5-4900-4D58-AD4A-EFC03EFD713B"))
                    return;
            }
            context.Result = new BadRequestObjectResult("missing token or wrong token");
        }
    }
}
