using System;
using AppComponent.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCommon.Extensions;
using Common.Extensions;
using TodoCenterProxyApi.Services;
using TodoCenterProxyApi.Extensions;

namespace TodoCenterProxyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoneListController : ControllerBase
    {
        private readonly IEnumerable<ITodoService> _services;

        public DoneListController(IEnumerable<ITodoService> services)
        {
            _services = services;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoListVM>>> GetAsync()
        {
            var taskes = new List<Task<IList<TodoListVM>>>();
            foreach (var service in _services)
            {
                taskes.Add(this.GetDoneListAsync(service));
            }
            await Task.WhenAll(taskes);
            var Donelist = taskes.SelectMany(u => u.Result).ToList();
            return Donelist;
        }

        private async Task<IList<TodoListVM>> GetDoneListAsync(ITodoService todoService)
        {
            try
            {
                var doneList = await todoService.GetCompleteAsync(this.HttpContext.DecodeUserClaimsFromRequestHeaders());
                return doneList;
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.Message("Get PendingList Faild,Reason:{0}", e.ToString());
                return new List<TodoListVM>();
            }
        }
    }
}
