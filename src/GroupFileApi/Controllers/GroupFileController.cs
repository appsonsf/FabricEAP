using Attachment;
using Attachment.Entities;
using Common;
using EnterpriseContact;
using EnterpriseContact.Services;
using GroupFile.Dtos;
using GroupFile.Extensions;
using InstantMessage;
using InstantMessage.Entities;
using InstantMessage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupFile.Controllers
{
    //群组验证
    [Route("api/v1/groupfile")]
    [Produces("application/json")]
    public class GroupFileController : EapBaseController
    {
        private readonly IControlAppService _groupFileControlApp;
        private readonly Func<string, IAttachmentAppService> _attachmentAppServiceFactory;
        private readonly Func<Guid, IConversationMsgAppService> _conversationMsgAppServiceFactory;
        private readonly ILogger _logger;
        private readonly IGroupAppService _groupAppService;
        private readonly IDepartmentAppService _departmentAppService;
        private readonly IEmployeeCacheService _employeeCacheService;
        private readonly IMemoryCache _cache;

        private const int MaxAttachmentSize = 10 * 1024 * 1024;

        public GroupFileController(IControlAppService controlAppService,
            IEmployeeCacheService employeeCacheService,
            IGroupAppService groupAppService,
            IDepartmentAppService departmentAppService,
            IMemoryCache cache,
            Func<string, IAttachmentAppService> attachmentAppServiceFactory,
            Func<Guid, IConversationMsgAppService> conversationMsgAppServiceFactory)
        {
            _groupFileControlApp = controlAppService;
            _attachmentAppServiceFactory = attachmentAppServiceFactory;
            _groupAppService = groupAppService;
            _conversationMsgAppServiceFactory = conversationMsgAppServiceFactory;
            _cache = cache;
            _logger = Log.ForContext<GroupFileController>();
            _departmentAppService = departmentAppService;
            _employeeCacheService = employeeCacheService;
        }

        [Route("{groupId}/files")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileItemDto>>> GetGroupFilesAsync(Guid groupId, bool depGroup, int page, int size)
        {
            try
            {
                if (Guid.Empty.Equals(groupId) || page < 1)
                    return null;
                var uploaderId = GetEmployeeId();
                var group = !depGroup
                    ? await _groupAppService.GetByIdAsync(groupId)
                    : await _departmentAppService.GetDepGroupByIdAsync(groupId);
                if (group == null)
                    return null;
                if (!IsMemeberInGroup(group.Members, uploaderId))
                    return null;
                var fileItems = await _groupFileControlApp.GetFileItemsAsync(groupId, page, size);
                if (fileItems != null)
                {
                    #region 获取上传者信息
                    var employeeIds = fileItems.DistinctBy(f => new { f.UploaderId })
                        .Select(x => x.UploaderId).ToArray();

                    var employees = await _employeeCacheService.ByIdAsync(employeeIds);

                    foreach (var item in fileItems)
                    {
                        var employee = employees.Find(x => x.Id == item.UploaderId);
                        if (employee != null)
                            item.UploaderName = employee.Name;
                    }
                    #endregion

                    #region 获取文件信息
                    var fileMd5s = fileItems.DistinctBy(f => new { f.StoreId });
                    List<Task<AttachmentItem>> attTasks = new List<Task<AttachmentItem>>();
                    foreach (var item in fileMd5s)
                    {
                        attTasks.Add(_attachmentAppServiceFactory(item.StoreId).GetByIdAsync(item.StoreId));
                    }
                    await Task.WhenAll(attTasks);
                    attTasks.ForEach(t =>
                    {
                        var e = t.Result;
                        if (e != null)
                        {
                            var fs = fileItems.FindAll(f => f.StoreId.Equals(e.Id));
                            if (fs != null)
                                foreach (var f in fs) f.Size = e.Size;
                        }
                    });
                    #endregion
                }
                return fileItems;
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }


        [Route("upload/{groupId}")]
        [HttpPut]
        public async Task<ActionResult<ResponseData<FileItemDto>>> UploadFileAsync(Guid groupId, string storeId, string fileName, bool depGroup = false)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(storeId) || Guid.Empty.Equals(groupId))
                    return ResponseData<FileItemDto>.BuildFailedResponse(message: "参数错误");
                var uploaderId = GetEmployeeId();
                var group = !depGroup
                    ? await _groupAppService.GetByIdAsync(groupId)
                    : await _departmentAppService.GetDepGroupByIdAsync(groupId);
                if (group == null)
                    return ResponseData<FileItemDto>.BuildFailedResponse(message: "群组不存在");
                if (!IsMemeberInGroup(group.Members, uploaderId))
                    return ResponseData<FileItemDto>.BuildFailedResponse(message: "无上传权限");
                var attachment = await _attachmentAppServiceFactory(storeId).GetByIdAsync(storeId);
                if (attachment == null)
                    return ResponseData<FileItemDto>.BuildFailedResponse(message: "文件上传失败");
                var input = new UploadInput
                {
                    FileName = fileName,
                    GroupId = groupId,
                    StoreId = storeId
                };
                await SendMessageAsync(groupId, GetUserId(), fileName, attachment);
                var result = await _groupFileControlApp.UploadFileAsync(input, uploaderId);
                _cache.Remove(groupId);
                if (result != null)
                {
                    result.Size = attachment.Size;
                    result.UploaderName = GetUserFullName();
                    return ResponseData<FileItemDto>.BuildSuccessResponse(result);
                }
                else
                    return ResponseData<FileItemDto>.BuildFailedResponse();
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        [Route("delete/{groupId}/{fileId}")]
        [HttpPut]
        public async Task<ActionResult<ResponseData>> Delete(Guid groupId, Guid fileId, bool depGroup = false)
        {
            try
            {
                var operatorId = GetEmployeeId();
                var group = !depGroup
                            ? await _groupAppService.GetByIdAsync(groupId)
                            : await _departmentAppService.GetDepGroupByIdAsync(groupId);
                if (group == null)
                    return ResponseData.BuildFailedResponse(message: "群组不存在");
                if (IsGroupOwner(group.Members, operatorId) || await IsFileOwnerAsync(fileId, operatorId))
                {
                    await _groupFileControlApp.DeleteFileItemAsync(fileId);
                    _cache.Remove(groupId);
                    return ResponseData.BuildSuccessResponse();
                }
                else
                {
                    return ResponseData.BuildFailedResponse(message: "没有操作权限");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        /// <summary>
        /// 返回attachment的id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [Route("download/{fileId}")]
        [HttpGet]
        public async Task<ActionResult<string>> DownloadFileAsync(Guid fileId)
        {
            if (Guid.Empty.Equals(fileId))
                return string.Empty;

            try
            {
                return await this._groupFileControlApp.DownloadFileAsync(fileId);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 群组附件容量剩余
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Route("freespace/{groupId}")]
        [HttpGet]
        public async Task<ActionResult<FreeSpaceDto>> FreeSpaceAsync(Guid groupId)
        {
            if (this._cache.TryGetValue<FreeSpaceDto>(groupId, out var freeSpaceDto))
            {
                return freeSpaceDto;
            }
            var groupFiles = await this._groupFileControlApp.GetFileItemsAsync(groupId, 1, int.MaxValue);
            var storeIds = groupFiles.Select(u => u.StoreId).ToArray();
            int stordBytes = 0;
            foreach (var storeId in storeIds)
            {
                var attachment = await this._attachmentAppServiceFactory.Invoke(storeId).GetByIdAsync(storeId);
                if (attachment != null)
                    stordBytes += attachment.Size;
            }
            var freeSpace = MaxAttachmentSize - stordBytes;
            freeSpaceDto = new FreeSpaceDto()
            {
                TotalSpace = MaxAttachmentSize,
                FreeSpace = freeSpace < 0 ? 0 : freeSpace
            };
            this._cache.Set(groupId, freeSpaceDto);
            return freeSpaceDto;
        }

        private async Task<GroupOutput> GetGroupAsync(Guid groupId)
        {
            return await _groupAppService.GetByIdAsync(groupId);
        }

        private async Task SendMessageAsync(Guid groupId, Guid senderId, string fileName, AttachmentItem attachment)
        {
            var fileMsg = new FileMessageVm
            {
                FileGuid = attachment.Id,
                FileName = fileName,
                FileSize = attachment.Size,
            };
            var msg = ConversationMsg.Create(groupId, senderId, fileMsg.GetJsonContent(), ConversationMsgType.File);
            var conversationMsgAppService = _conversationMsgAppServiceFactory(msg.ConversationId);
            await conversationMsgAppService?.SendMessageAsync(msg);
        }

        private bool IsGroupOwner(List<GroupMemberOutput> memebers, Guid employeeId)
        {
            if (memebers?.Count == 0)
                return false;
            var u = memebers?.First((m) => employeeId.Equals(m?.EmployeeId));
            return u != null && u.IsOwner;
        }

        private bool IsMemeberInGroup(List<GroupMemberOutput> memebers, Guid employeeId)
        {
            if (memebers?.Count == 0)
                return false;
            var u = memebers?.First((m) => employeeId.Equals(m?.EmployeeId));
            return u != null;
        }

        private async Task<bool> IsFileOwnerAsync(Guid fileId, Guid employeeId)
        {
            var fileItem = await _groupFileControlApp.GetFileItemAsync(fileId);
            return fileItem?.UploaderId == employeeId;
        }
    }
}
