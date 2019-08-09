using System;

namespace GroupFile.Dtos
{
    /// <summary>
    /// 上传参数
    /// </summary>
    public class UploadInput
    {
        /// <summary>
        /// 文件存储Id
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 群组Id
        /// </summary>
        public Guid GroupId { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
    }
}
