using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Attachment.Entities
{
    [Serializable]
    [DataContract]
    public class AttachmentItem
    {
        /// <summary>
        /// 文件的md5值
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// 存储的KB，显示的时候需要可能显示为MB，GB等
        /// </summary>
        [DataMember]
        public int Size { get; set; }

        /// <summary>
        /// 存储服务中的路径，比如：attachments/2018/12/28/{id}.ext
        /// </summary>
        [DataMember]
        public string Location { get; set; }

        /// <summary>
        /// 初次上传时间
        /// </summary>
        [DataMember]
        public DateTimeOffset Uploaded { get; set; }

        /// <summary>
        /// 初次上传者的UserId
        /// </summary>
        [DataMember]
        public Guid UploadBy { get; set; }

        /// <summary>
        /// 此附件的上下文Id，比如IM的ConversationId，如果不属于任何上下文就设置为Empty；
        /// 添加后不可修改
        /// </summary>
        [DataMember]
        public Guid ContextId { get; set; } //index

        [DataMember]
        public UploadStatus Status { get; set; }
    }
}
