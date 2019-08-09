using System;

namespace GroupFile
{
    public class FileItemDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTimeOffset UpdatedOn { get; set; }

        /// <summary>
        /// 上传者的在ECS中的员工Id（也是主数据中的员工Id）
        /// </summary>
        public Guid UploaderId { get; set; }

        public string UploaderName { get; set; }

        public int DownloadAmount { get; set; }

        public string StoreId { get; set; }

        public int Size { get; set; }
    }
}
