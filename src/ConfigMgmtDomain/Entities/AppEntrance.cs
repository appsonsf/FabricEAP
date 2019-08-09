using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConfigMgmt.Entities
{
    [DataContract]
    public class AppEntrance
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Icon { get; set; }

        /// <summary>
        /// 关联文件夹Id
        /// </summary>
        [DataMember]
        public Guid? FolderId { get; set; }

        /// <summary>
        /// 是否是文件夹的配置
        /// </summary>
        [DataMember]
        public bool IsFolder { get; set; }

        /// <summary>
        /// 应用Id 下载需要
        /// </summary>
        [DataMember]
        public string AppId { get; set; } = "";

        /// <summary>
        /// 图标显示尺寸比例，1=1x，2=高2x，3=宽2x，4=高宽2x
        /// </summary>
        [DataMember]
        public int IconDisplaySize { get; set; }

        [DataMember]
        public int Order { get; set; }

        [DataMember]
        public AppEntryAuthType AuthType { get; set; }

        /// <summary>
        /// 显示和进入此应用入口需要的系统角色；
        /// 如果为empty，那么任何人皆可进入；
        /// 如果有多个，需要都满足
        /// </summary>
        [DataMember]
        public string[] NeedRoles { get; set; }

        [DataMember]
        public bool NeedAuth { get; set; }

        /// <summary>
        /// 组件配置信息，只有入口是组件的才需要
        /// </summary>
        [DataMember]
        public ComponentConfig ComponentConfig { get; set; }

        /// <summary>
        /// 不同客户端平台上的启动Uri
        /// </summary>
        [DataMember]
        public Dictionary<ClientPlatform, string> Uris { get; set; }
    }
}
