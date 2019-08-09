using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ConfigMgmt.Entities
{
    /// <summary>
    /// 组件配置信息
    /// </summary>
    [DataContract]
    public class ComponentConfig
    {
        [DataMember]
        public ComponentDataSourceAuthType AuthType { get; set; }

        /// <summary>
        /// 验证用户名，根据不同验证方式保存不同语义的内容；
        /// when AuthType==ClientCredential is ClientId
        /// </summary>
        [DataMember]
        public string AuthUsername { get; set; }

        /// <summary>
        /// 验证密码，根据不同验证方式保存不同语义的内容；
        /// when AuthType==ClientCredential is ClientSecret
        /// </summary>
        [DataMember]
        public string AuthPassword { get; set; }

        /// <summary>
        /// 是否显示角标
        /// </summary>
        [DataMember]
        public bool ShowBadge { get; set; }

        /// <summary>
        /// 组件数据源接口，可以多条
        /// </summary>
        [DataMember]
        public List<ComponentDataSource> DataSources { get; set; }
    }
}
