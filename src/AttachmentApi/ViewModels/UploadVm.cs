using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Attachment.ViewModels
{
    public class UploadVm
    {
        /// <summary>
        /// 上传的业务模块上下文Id，可以为Empty
        /// </summary>
        public Guid ContextId { get; set; }
        /// <summary>
        /// 文件的md5值
        /// </summary>
        public string MD5 { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public IFormFile File { get; set; }
    }
}
