using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ConfigMgmt.Entities
{
    /// <summary>
    /// 移动平台客户端，可以多个客户端实例
    /// </summary>
    [DataContract]
    public class Client
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 支持的客户端平台
        /// </summary>
        [DataMember]
        public ClientPlatform SupportPlatforms { get; set; }

        [DataMember]
        public DateTimeOffset Created { get; set; }

        [DataMember]
        public DateTimeOffset? Modified { get; set; }

        [DataMember]
        public DateTimeOffset? Deleted { get; set; }

        [DataMember]
        public List<Guid> AppEntranceIds { get; set; }

        [DataMember]
        public DateTimeOffset? WorkbenchConfigTimestamp { get; set; }

        public const string DefaultId = "default";

        public static Client Default()
        {
            return new Client
            {
                Id = DefaultId,
                Name = "默认",
                SupportPlatforms = ClientPlatform.Android | ClientPlatform.Desktop | ClientPlatform.iOS,
                Created = DateTimeOffset.UtcNow,
                AppEntranceIds = new List<Guid>()
            };
        }
    }
}
