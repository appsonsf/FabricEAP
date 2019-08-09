using System;

namespace GroupFile.Entities
{
    public class FileItem
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 文件归属群组Id，来自ECS
        /// </summary>
        //[Index]
        public Guid GroupId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTimeOffset UpdatedOn { get; set; }

        /// <summary>
        /// 上传者的在ECS中的员工Id（也是主数据中的员工Id）
        /// </summary>
        public Guid UploaderId { get; set; }

        public int DownloadAmount { get; set; }

        public string StoreId { get; set; }

    }
}
