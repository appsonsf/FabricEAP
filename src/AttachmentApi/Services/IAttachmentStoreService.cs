using Attachment.Entities;
using Attachment.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Attachment.Services
{
    public interface IAttachmentStoreService
    {
        Task FillMemoryStreamAsync(AttachmentItem dto, MemoryStream ms);

        Task TryDeleteFileAsync(string objectName);

        Task<string> StoreFileAsync(UploadVm model, DateTimeOffset now);
    }
}
