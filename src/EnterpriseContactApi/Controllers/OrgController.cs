using Common;
using AutoMapper;
using EnterpriseContact.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace EnterpriseContact.Controllers
{
    [Route("api/v1/org")]
    public class OrgController : TheBaseController
    {
        private readonly IEmployeeAppService _employeeAppService;
        private readonly ILogger _logger;

        public OrgController(IMemoryCache cache, IMapper mapper,
            IDepartmentAppService departmentAppService, IPositionAppService positionAppService,
            IEmployeeAppService employeeAppService) 
            : base(cache, mapper, departmentAppService, positionAppService)
        {
            _employeeAppService = employeeAppService;
            _logger = Log.ForContext<OrgController>();
        }

        [HttpGet("root")]
        public async Task<ActionResult<OrgVm>> GetRoots()
        {
            try
            {
                if (!_cache.TryGetValue(CacheKeys.EnterpriseContact_GetRoots, out OrgVm data))
                {
                    var departments = await _departmentAppService.GetRootListAsync();
                    var employees = await _employeeAppService.GetRootListAsync();
                    data = new OrgVm
                    {
                        Departments = _mapper.Map<List<DepartmentListVm>>(departments),
                        Employees = _mapper.Map<List<EmployeeListVm>>(employees)
                    };

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(CacheKeys.DefaultCacheAbsoluteExpiration);

                    _cache.Set(CacheKeys.EnterpriseContact_GetRoots, data, cacheEntryOptions);
                }

                return data;
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        [HttpGet("{parentDepartmentId}")]
        public async Task<ActionResult<OrgVm>> GetChildren(Guid parentDepartmentId)
        {
            try
            {
                var departments = await _departmentAppService.GetListByParentIdAsync(parentDepartmentId);
                var employees = await _employeeAppService.GetListByDepartmentIdAsync(parentDepartmentId);
                var data = new OrgVm
                {
                    Departments = _mapper.Map<List<DepartmentListVm>>(departments),
                    Employees = _mapper.Map<List<EmployeeListVm>>(employees)
                };

                return data;
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }
    }
}
