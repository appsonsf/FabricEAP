using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConfigMgmt.Entities
{
    /// <summary>
    /// 待办中心集成的业务系统
    /// </summary>
    [DataContract]
    public class TodoCenterBizSystem
    {
        public const string Id_OA = "oa";
        public const string Id_PM = "pm";
        public const string Id_KINDDEE = "kd";

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string AppId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string PendingListUrl { get; set; }

        [DataMember]
        public string PendingListAmountUrl { get; set; }

        [DataMember]
        public string DoneListUrl { get; set; }

        [DataMember]
        public Dictionary<ClientPlatform, string> OpenDetailUris { get; set; }
    }
}
