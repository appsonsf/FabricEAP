using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Attachment.Entities;
using Attachment.ViewModels;
using AppsOnSF.Common.Options;
using Microsoft.Extensions.Options;
using Minio;
using Serilog;

namespace Attachment.Services
{
    public class MinioAttachmentStoreService : IAttachmentStoreService
    {
        private readonly MinioOption _minioOption;
        private readonly MinioClient _client;
        private readonly ILogger _logger;

        public MinioAttachmentStoreService(IOptions<MinioOption> minioOptionAccessor)
        {
            _minioOption = minioOptionAccessor.Value;
            _client = new MinioClient(_minioOption.Endpoint, _minioOption.AccessKey, _minioOption.SecretKey);
            _logger = Log.ForContext<MinioAttachmentStoreService>();
        }

        public async Task FillMemoryStreamAsync(AttachmentItem dto, MemoryStream ms)
        {
            await _client.GetObjectAsync(_minioOption.BucketName, dto.Location,
                stream => stream.CopyTo(ms));
        }

        public async Task<string> StoreFileAsync(UploadVm model, DateTimeOffset now)
        {
            var fileName = model.File.FileName;
            var folder = $"attachments/{now.Year}/{now.Month}/{now.Day}/";
            var saveFileName = model.MD5 + Path.GetExtension(fileName);
            //attachments/2018/12/28/{id}.ext
            var objectName = folder + saveFileName;

            bool found = await _client.BucketExistsAsync(_minioOption.BucketName);
            if (!found)
            {
                await _client.MakeBucketAsync(_minioOption.BucketName);
            }
            using (var stream = model.File.OpenReadStream())
            {
                await _client.PutObjectAsync(_minioOption.BucketName, objectName, stream, model.File.Length);
            }
            return objectName;
        }

        public async Task TryDeleteFileAsync(string objectName)
        {
            try
            {
                await _client.RemoveObjectAsync(_minioOption.BucketName, objectName);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }
    }
}
