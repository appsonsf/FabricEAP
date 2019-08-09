using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConfigMgmt.Entities
{
    [DataContract]
    public class ComponentDataSource
    {
        public ComponentDataSource()
        {

        }

        public ComponentDataSource(string key, string value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// 组件数据源接口Key
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// 组件数据源接口地址
        /// </summary>
        [DataMember]
        public string Value { get; set; }

        /// <summary>
        /// 组件数据源接口描述
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// 组件数据源接口参数
        /// </summary>
        [DataMember]
        public Dictionary<string, string> Parameters { get; set; }
    }

    public static class ComponentDataSourceKeys
    {
        public const string BadgeValue = "badge_value";

        #region Todo component
        public const string TodoPendingList = "pending_list";
        public const string TodoDoneList = "done_list";
        #endregion
    }
}
