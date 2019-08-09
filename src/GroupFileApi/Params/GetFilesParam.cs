using System;

namespace GroupFile.Params
{
    public class GetFilesParam
    {
        /// <summary>
        /// 群组Id
        /// </summary>
        public Guid GroupId { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; } = 20;
    }
}
