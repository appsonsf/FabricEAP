using Attachment.Entities;
using Attachment.Services;
using Attachment.ViewModels;
using Common;
using AppsOnSF.Common.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Minio;
using Serilog;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Attachment.Controllers
{
    [Route("api/v1/attachment")]
    public class AttachmentController : EapBaseController
    {
        private readonly Func<string, IAttachmentAppService> _attachmentAppServiceFactory;
        private readonly IAttachmentStoreService _attachmentFileStoreService;
        private readonly IMemoryCache _cache;
        private readonly ILogger _logger;

        public AttachmentController(Func<string, IAttachmentAppService> attachmentAppServiceFactory,
            IAttachmentStoreService attachmentFileStoreService,
            IMemoryCache cache)
        {
            _attachmentAppServiceFactory = attachmentAppServiceFactory;
            _attachmentFileStoreService = attachmentFileStoreService;
            _cache = cache;
            _logger = Log.ForContext<AttachmentController>();
        }

        /// <summary>
        /// 判断附件是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("status/{id}")]
        public async Task<ActionResult<UploadStatus>> GetStatusAsync([Required]string id)
        {
            try
            {
                var appService = _attachmentAppServiceFactory(id);
                var dto = await appService.GetByIdAsync(id);
                if (dto == null) return UploadStatus.None;
                return dto.Status;
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        //[HttpPut("status/{id}")]
        //public async Task<IActionResult> UpdateStatusAsync([Required]string id, UploadStatus status)
        //{
        //    var result=await _attachmentAppService.UpdateStatusByIdAsync(id,status);
        //    if (result.IsSuccess) return Ok();
        //    return BadRequest();
        //}
        [HttpGet("thumbnails/{id}")]
        public async Task<IActionResult> DownloadThumbnailsAsync([Required]string id, string fileName)
        {
            try
            {
                var fileNameWithExt = string.IsNullOrEmpty(fileName) ? id + ".jpg" : fileName;
                if (_cache.TryGetValue(id, out byte[] thumbnailsData))
                    return File(thumbnailsData, "image/jpeg", fileNameWithExt);

                var appService = _attachmentAppServiceFactory(id);
                var dto = await appService.GetByIdAsync(id);
                if (dto == null) return NotFound();
                if (dto.Status < UploadStatus.Uploaded) return NotFound();
                if (string.IsNullOrEmpty(dto.Location)) return NotFound();

                using (var ms = new MemoryStream())
                {
                    await _attachmentFileStoreService.FillMemoryStreamAsync(dto, ms);

                    ms.Position = 0;
                    var thumbnailsImage = Image.FromStream(ms, false, false);
                    var thumbnailsStream = new MemoryStream();
                    ms.Position = 0;
                    PhotoResizer.Resize(ms, thumbnailsStream, thumbnailsImage.Width, thumbnailsImage.Height);
                    thumbnailsData = thumbnailsStream.ToArray();
                    _cache.Set(id, thumbnailsData, TimeSpan.FromMinutes(30));
                    return File(thumbnailsData, "image/jpeg", fileNameWithExt);
                    
                }
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadFileAsync([Required]string id, string fileName)
        {
            try
            {
                var appService = _attachmentAppServiceFactory(id);
                var dto = await appService.GetByIdAsync(id);
                if (dto == null) return NotFound();
                if (dto.Status < UploadStatus.Uploaded) return NotFound();
                if (string.IsNullOrEmpty(dto.Location)) return NotFound();

                //ref: https://scottsauber.com/2019/02/25/dynamically-setting-content-type-in-asp-net-core-with-fileextensioncontenttypeprovider/
                var fileProvider = new FileExtensionContentTypeProvider();
                var fileNameWithExt = string.IsNullOrEmpty(fileName) ? Path.GetFileName(dto.Location) : fileName;
                if (!fileProvider.TryGetContentType(fileNameWithExt, out string contentType))
                {
                    contentType = "application/octet-stream";
                }

                var ms = new MemoryStream();
                await _attachmentFileStoreService.FillMemoryStreamAsync(dto, ms);
                ms.Position = 0;
                return File(ms, contentType, fileNameWithExt);
            }
            catch (Exception ex)
            {
                return BadRequest(LogError(_logger, ex));
            }
        }

        //ref: https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-2.2

        [HttpPost]
        public async Task<ActionResult<ResponseData<UploadStatus>>> UploadFileAsync([FromForm]UploadVm model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.File == null || string.IsNullOrEmpty(model.MD5))
                return ResponseData<UploadStatus>.BuildFailedResponse(message: "参数不正确");

            var appService = _attachmentAppServiceFactory(model.MD5);
            var dto = await appService.GetByIdAsync(model.MD5);
            if (dto != null && dto.Status > UploadStatus.Uploading)
                return ResponseData<UploadStatus>.BuildSuccessResponse(dto.Status);

            var now = DateTimeOffset.UtcNow;
            string objectName;
            try
            {
                objectName = await _attachmentFileStoreService.StoreFileAsync(model, now);

                if (dto == null)
                {
                    dto = new AttachmentItem
                    {
                        ContextId = model.ContextId,
                        Id = model.MD5,
                    };
                }
                dto.Location = objectName;
                dto.Size = (int)(model.File.Length / 1000);
                dto.Status = UploadStatus.Uploaded;
                dto.UploadBy = GetUserId();
                dto.Uploaded = now;
                try
                {
                    await appService.AddOrUpdateAsync(dto);
                }
                catch (Exception e)
                {
                    await _attachmentFileStoreService.TryDeleteFileAsync(objectName);
                    return BadRequest(LogError(_logger,e));
                }
                return ResponseData<UploadStatus>.BuildSuccessResponse(dto.Status);
            }
            catch (Exception ex)
            {
                return ResponseData<UploadStatus>.BuildFailedResponse(message: LogError(_logger, ex));
            }
        }
    }
}
