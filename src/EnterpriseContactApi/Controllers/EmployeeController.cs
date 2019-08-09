using Common;
using AutoMapper;
using ConfigMgmt;
using EnterpriseContact.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Services;
using Serilog;

namespace EnterpriseContact.Controllers
{
    [Route("api/v1/employee")]
    public class EmployeeController : TheBaseController
    {
        private readonly IEmployeeAppService _employeeAppService;
        private readonly IGroupAppService _groupAppService;
        private readonly Func<Guid, IUserFavoriteAppService> _userFavoriteAppServiceFactory;
        private readonly Func<Guid, IUserSettingAppService> _userSettingAppServiceFactory;
        private readonly ILogger _logger;

        public EmployeeController(IMemoryCache cache, IMapper mapper,
            IDepartmentAppService departmentAppService, IPositionAppService positionAppService,
            IEmployeeAppService employeeAppService,
            IGroupAppService groupAppService,
            Func<Guid, IUserFavoriteAppService> userFavoriteAppServiceFactory,
            Func<Guid, IUserSettingAppService> userSettingAppServiceFactory
            )
            : base(cache, mapper, departmentAppService, positionAppService)
        {
            _employeeAppService = employeeAppService;
            _groupAppService = groupAppService;
            _userFavoriteAppServiceFactory = userFavoriteAppServiceFactory;
            _userSettingAppServiceFactory = userSettingAppServiceFactory;
            _logger = Log.ForContext<EmployeeController>();
        }

        [HttpGet("my")]
        public async Task<ActionResult<List<EmployeeListVm>>> GetMy()
        {
            try
            {
                var userId = GetUserId();

                await EnsureDepartmentListCacheAsync();
                await EnsurePositionListCacheAsync();

                var favs = await _userFavoriteAppServiceFactory(userId).GetEmployeesAsync(userId);
                var dto = await _employeeAppService.GetListByIdsAsync(favs);
                return GenerateEmployeeList(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        [HttpPost("fav/{employeeId}")]
        public async Task<ActionResult<ResponseData>> AddFavorite(Guid employeeId)
        {
            try
            {
                var userId = GetUserId();

                if (await _userFavoriteAppServiceFactory(userId).AddEmployeeAsync(userId, employeeId))
                    return BuildSuccess();
                return BuildFaild();
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        [HttpDelete("fav/{employeeId}")]
        public async Task<ActionResult<ResponseData>> RemoveFavorite(Guid employeeId)
        {
            try
            {
                var userId = GetUserId();

                if (await _userFavoriteAppServiceFactory(userId).RemoveEmployeeAsync(userId, employeeId))
                    return BuildSuccess();
                return BuildFaild();
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<EmployeeListVm>>> Search(string keyword)
        {
            try
            {
                await EnsureDepartmentListCacheAsync();
                await EnsurePositionListCacheAsync();

                var dto = await _employeeAppService.SearchByKeywordAsync(keyword);
                return GenerateEmployeeList(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        private ActionResult<List<EmployeeListVm>> GenerateEmployeeList(List<EmployeeListOutput> dto)
        {
            var data = new List<EmployeeListVm>();
            foreach (var item in dto)
            {
                var vm = _mapper.Map<EmployeeListOutput, EmployeeListVm>(item, (opts) =>
                {
                    opts.AfterMap((EmployeeListOutput s, EmployeeListVm d) =>
                    {
                        if (_positionListCache.ContainsKey(s.PrimaryPositionId))
                            d.PositionName = _positionListCache[s.PrimaryPositionId].Name;
                        SetDepartmentNames(s, d);
                        d.IsPrimary = true;
                    });
                });
                data.Add(vm);
            }

            return data;
        }

        //最多设置两级部门名称
        private void SetDepartmentNames(EmployeeListOutput s, EmployeeListVm d)
        {
            d.DepartmentNames = new List<string>();
            if (_departmentListCache.ContainsKey(s.PrimaryDepartmentId))
            {
                var item = _departmentListCache[s.PrimaryDepartmentId];
                d.DepartmentNames.Add(item.Name);
                if (item.ParentId.HasValue && _departmentListCache.ContainsKey(item.ParentId.Value))
                {
                    d.DepartmentNames.Add(_departmentListCache[item.ParentId.Value].Name);
                }
            }
        }

        private async Task<ActionResult<EmployeeDetailVm>> ReturnByOutputAsync(Guid id, Func<Guid, Task<EmployeeOutput>> getOutput)
        {
            var dto = await getOutput(id);
            if (dto == null) return NotFound(id);

            await EnsureDepartmentListCacheAsync();
            await EnsurePositionListCacheAsync();

            var vm = _mapper.Map<EmployeeOutput, EmployeeDetailVm>(dto, (opts) =>
            {
                opts.AfterMap((EmployeeOutput s, EmployeeDetailVm d) =>
                {
                    if (_positionListCache.ContainsKey(s.PrimaryPositionId))
                        d.PositionName = _positionListCache[s.PrimaryPositionId].Name;
                    SetDepartmentNames(s, d);
                    SetParttimeJobs(s, d);
                });
            });

            var userId = GetUserId();
            var employeeId = GetEmployeeId();

            var task1 = _groupAppService.CheckSameWhiteListGroupAsync(employeeId, id);
            var task2 = _userFavoriteAppServiceFactory(userId).IsFavoritedAsync(userId, id);
            var task3 = dto.UserId.HasValue
                ? _userSettingAppServiceFactory(userId).GetInfoVisibilityAsync(dto.UserId.Value)
                : Task.FromResult(new InfoVisibility());
            await Task.WhenAll(task1, task2, task3);

            vm.SameWhiteListGroup = task1.Result;
            vm.IsFavorited = task2.Result;

            if (!task3.Result.Mobile)
                vm.Mobile = "***";

            return vm;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeDetailVm>> GetById(Guid id)
        {
            try
            {
                return await ReturnByOutputAsync(id, (o) => _employeeAppService.GetByIdAsync(o));
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        [HttpGet("userId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeDetailVm>> GetByUserId(Guid id)
        {
            try
            {
                return await ReturnByOutputAsync(id, (o) => _employeeAppService.GetByUserIdAsync(o));
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        private void SetParttimeJobs(EmployeeOutput s, EmployeeDetailVm d)
        {
            d.ParttimeJobs = new List<ParttimeJobListVm>();
            foreach (var posId in s.ParttimePositionIds)
            {
                if (_positionListCache.ContainsKey(posId))
                {
                    var pos = _positionListCache[posId];
                    var vm = new ParttimeJobListVm();
                    vm.PositionName = pos.Name;
                    vm.DepartmentNames = new List<string>();
                    //最多设置两级部门名称
                    if (_departmentListCache.ContainsKey(pos.DepartmentId))
                    {
                        var dep = _departmentListCache[pos.DepartmentId];
                        vm.DepartmentNames.Add(dep.Name);
                        if (dep.ParentId.HasValue && _departmentListCache.ContainsKey(dep.ParentId.Value))
                        {
                            vm.DepartmentNames.Add(_departmentListCache[dep.ParentId.Value].Name);
                        }
                    }
                    d.ParttimeJobs.Add(vm);
                }
            }
        }

        //返回完整层级
        private void SetDepartmentNames(EmployeeOutput s, EmployeeDetailVm d)
        {
            d.DepartmentNames = new List<string>();
            if (_departmentListCache.ContainsKey(s.PrimaryDepartmentId))
            {
                var item = _departmentListCache[s.PrimaryDepartmentId];
                d.DepartmentNames.Add(item.Name);
                while (item.ParentId.HasValue && _departmentListCache.ContainsKey(item.ParentId.Value))
                {
                    var sub = _departmentListCache[item.ParentId.Value];
                    d.DepartmentNames.Add(sub.Name);
                    item = sub;
                }
            }
        }

        [HttpGet("checkSameWhiteList")]
        public async Task<ActionResult<bool>> CheckSameWhiteListGroup(Guid employeeId)
        {
            try
            {
                return await _groupAppService.CheckSameWhiteListGroupAsync(GetEmployeeId(), employeeId);
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }
    }
}
