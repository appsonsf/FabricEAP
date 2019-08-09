using System;
using System.Collections.Generic;
using System.Text;

namespace Attachment
{
    /// <summary>
    /// 附件文件上传的状态，如果为Uploaded，即无需重复上传
    /// </summary>
    public enum UploadStatus
    {
        /// <summary>
        /// 不存在
        /// </summary>
        None = 0,
        /// <summary>
        /// 添加成功了Attachment的数据项，等待上传文件
        /// </summary>
        Waiting = 1,
        /// <summary>
        /// 正在上传文件
        /// </summary>
        Uploading = 2,
        /// <summary>
        /// 上传文件成功
        /// </summary>
        Uploaded = 3,
        /// <summary>
        /// 附件被业务模块所使用，比如Conversation或者GroupFile
        /// </summary>
        InUsed = 4
    }
}
