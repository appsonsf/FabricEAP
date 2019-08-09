using Common;
using AutoMapper;
using EnterpriseContact.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceFabricContrib;
using Serilog;

namespace EnterpriseContact.Controllers
{
    [Route("api/v1/group")]
    public class GroupController : TheBaseController
    {
        private readonly IGroupAppService _groupAppService;
        private readonly ILogger _logger;

        public GroupController(IMemoryCache cache, IMapper mapper,
            IDepartmentAppService departmentAppService, IPositionAppService positionAppService,
            IGroupAppService groupAppService)
            : base(cache, mapper, departmentAppService, positionAppService)
        {
            _groupAppService = groupAppService;
            _logger = Log.ForContext<GroupController>();
        }

        [HttpGet("my")]
        public async Task<ActionResult<List<GroupListVm>>> GetMy()
        {
            try
            {
                var employeeId = GetEmployeeId();

                var dto = await _groupAppService.GetListByEmployeeIdAsync(employeeId);
                var data = _mapper.Map<List<GroupListVm>>(dto);

                return data;
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GroupDetailVm>> GetById(Guid id, bool depGroup = false)
        {
            try
            {
                await EnsureDepartmentListCacheAsync();
                await EnsurePositionListCacheAsync();

                var dto = !depGroup
                    ? await _groupAppService.GetByIdAsync(id)
                    : await _departmentAppService.GetDepGroupByIdAsync(id);

                if (dto == null) return NotFound(id);
                var data = _mapper.Map<GroupDetailVm>(dto);
                if (dto.Members != null && data != null)
                {
                    foreach (var s in dto.Members)
                    {
                        var d = data.Members.First((item) => item.EmployeeId.Equals(s.EmployeeId));
                        if (d != null) SetDepartmentNames(s, d);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        //最多设置两级部门名称
        private void SetDepartmentNames(GroupMemberOutput s, GroupMemberListVm d)
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

        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseData<Guid>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ResponseData<Guid>>> Create(GroupEditVm model)
        {

            try
            {
                var input = _mapper.Map<GroupEditVm, GroupInput>(model, opts =>
            {
                opts.AfterMap((GroupEditVm s, GroupInput d) =>
                {
                    d.CurrentUserId = GetUserId();
                    d.CurrentEmployeeId = GetEmployeeId();
                });
            });

                var id = await _groupAppService.CreateCustomAsync(input);
                return CreatedAtAction("GetById", new { id }, BuildSuccess(id));
                //return BuildSuccess(id);
                //TODO 这里可能要根据客户端的需求返回一个VM
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        /// <summary>
        /// 修改群组信息（名称等）,编辑成员
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseData>> Update(Guid id, GroupEditVm model)
        {
            try
            {
                var input = _mapper.Map<GroupEditVm, GroupInput>(model, opts =>
                {
                    opts.AfterMap((GroupEditVm s, GroupInput d) =>
                    {
                        d.Id = id;
                        d.CurrentUserId = GetUserId();
                        d.CurrentEmployeeId = GetEmployeeId();
                    });
                });

                var result = await _groupAppService.UpdateAsync(input);
                if (result.IsSuccess)
                    return BuildSuccess();
                var msg = "未知业务错误";
                if (result.FailedCode == FailedCodes.Group_NotCreatedBy)
                    msg = "不是所有者，不能编辑/添加成员/删除成员";
                return BuildFaild(result.FailedCode, msg);
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        /// <summary>
        /// 删除/解散群组
        /// 只有Owner才行
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseData>> Delete(Guid id)
        {
            try
            {
                var result = await _groupAppService.DeleteAsync(new GroupInput
                {
                    Id = id,
                    CurrentUserId = GetUserId(),
                });
                if (result.IsSuccess)
                    return BuildSuccess();
                var msg = "未知业务错误";
                if (result.FailedCode == FailedCodes.Group_NotCreatedBy)
                    msg = "不是所有者，不能解散群组";
                return BuildFaild(result.FailedCode, msg);
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        /// <summary>
        /// 退出群组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("quit/{id}")]
        public async Task<ActionResult<ResponseData>> Quit(Guid id)
        {
            try
            {
                var result = await _groupAppService.QuitAsync(new GroupInput
                {
                    Id = id,
                    CurrentUserId = GetUserId(),
                    CurrentEmployeeId = GetEmployeeId(),
                    CurrentEmployeeName = GetUserFullName(),
                });
                if (result.IsSuccess)
                    return BuildSuccess();
                return BuildFaild();
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        [HttpGet("scanjoin/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseData<string>>> GetScanJoinCode(Guid id)
        {
            try
            {
                var r = await _groupAppService.CreateScanJoinCodeAsync(id, GetUserId());
                if (!r.IsSuccess) return BadRequest(r.FailedCode);
                return BuildSuccess(r.Output);
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        [HttpPut("scanjoin/{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseData>> ScanJoin(string code)
        {
            try
            {
                var r = await _groupAppService.ScanJoinAsync(code, GetEmployeeId(), GetUserFullName());
                if (!r.IsSuccess) return BadRequest(r.FailedCode);
                return BuildSuccess();
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }
    }
}
