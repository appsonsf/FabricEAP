using Microsoft.AspNetCore.Http;
using System;

namespace GroupFile.Params
{
    /// <summary>
    /// 文件上传参数
    /// </summary>
    public class UploadParam
    {
        /// <summary>
        /// 群组id
        /// </summary>
        public Guid GroupId { get; set; }
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
