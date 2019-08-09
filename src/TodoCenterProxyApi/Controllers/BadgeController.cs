using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCommon.Extensions;
using AppComponent.ViewModels;
using Common.Extensions;
using TodoCenterProxyApi.Services;
using TodoCenterProxyApi.Extensions;

namespace TodoCenterProxyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BadgeController : ControllerBase
    {
        private readonly IEnumerable<ITodoService> _services;

        public BadgeController(IEnumerable<ITodoService> services)
        {
            _services = services;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<int>> GetAsync()
        {
            var taskes = new List<Task<int>>();
            foreach (var service in _services)
            {
                taskes.Add(this.GetUnReadCountAsync(service));
            }
            await Task.WhenAll(taskes);
            var unread_count = taskes.Sum(u => u.Result);
            return unread_count;
        }

        private async Task<int> GetUnReadCountAsync(ITodoService todoService)
        {
            try
            {
                var doneList = await todoService.GetUnReadCountAsync(this.HttpContext.DecodeUserClaimsFromRequestHeaders());
                return doneList;
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.Message("Get UnReadCount Faild,Reason:{0}", e.ToString());
                return 0;
            }
        }
    }
}
