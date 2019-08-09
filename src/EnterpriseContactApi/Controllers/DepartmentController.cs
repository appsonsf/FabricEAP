using Common;
using AutoMapper;
using EnterpriseContact.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;

namespace EnterpriseContact.Controllers
{
    [Route("api/v1/department")]
    public class DepartmentController : TheBaseController
    {
        private readonly ILogger _logger;

        public DepartmentController(IMemoryCache cache, IMapper mapper, IDepartmentAppService departmentAppService, IPositionAppService positionAppService) 
            : base(cache, mapper, departmentAppService, positionAppService)
        {
            _logger = Log.ForContext<DepartmentController>();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<DepartmentSearchListVm>>> Search(string keyword)
        {
            try
            {
                await EnsureDepartmentListCacheAsync();

                var dto = await _departmentAppService.SearchByKeywordAsync(keyword);
                var data = new List<DepartmentSearchListVm>();
                foreach (var item in dto)
                {
                    var vm = _mapper.Map<DepartmentListOutput, DepartmentSearchListVm>(item, (opts) =>
                    {
                        opts.AfterMap((s, d) =>
                        {
                            if (s.ParentId.HasValue && _departmentListCache.ContainsKey(s.ParentId.Value))
                                d.ParentName = _departmentListCache[s.ParentId.Value].Name;
                        });
                    });
                    data.Add(vm);
                }

                return data;
            }
            catch (System.Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }
    }
}
