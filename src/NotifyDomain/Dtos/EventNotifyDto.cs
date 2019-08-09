using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Notification
{
    [DataContract]
    public class EventNotifyDto : NotifyDto
    {

        /// <summary>
        /// 事件提醒标题（可选）
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 事件提醒内容文本
        /// </summary>
        [DataMember]
        public string Text { get; set; }

        /// <summary>
        /// 事件提醒的内容参数
        /// </summary>
        [DataMember]
        public Dictionary<string, string> Parameters { get; set; }

        [DataMember]
        public DateTimeOffset Created { get; set; }
    }


}
