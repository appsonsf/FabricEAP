using System;
using AppComponent.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoCenterProxyApi.Services;
using ApiCommon.Extensions;
using Common.Extensions;
using TodoCenterProxyApi.Extensions;

namespace TodoCenterProxyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PendingListController : ControllerBase
    {
        private readonly IEnumerable<ITodoService> _todoServices;

        public PendingListController(IEnumerable<ITodoService> todoServices)
        {
            _todoServices = todoServices;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoListVM>>> GetAsync()
        {
            
            var taskes = new List<Task<IList<TodoListVM>>>();
            foreach (var service in _todoServices)
            {
                taskes.Add(this.GetPendingListAsync(service));
            }
            await Task.WhenAll(taskes);
            var todoes = taskes.SelectMany(u => u.Result).ToList();
            return todoes;
        }

        private async Task<IList<TodoListVM>> GetPendingListAsync(ITodoService service)
        {
            try
            {
                var todoes = await service.GetAsync(this.HttpContext.DecodeUserClaimsFromRequestHeaders());
                return todoes;
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.Message("Get PendingList Faild,Reason:{0}", e.ToString());
                return new List<TodoListVM>();
            }
        }
    }
}
