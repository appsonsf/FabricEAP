using System;
using System.Collections.Generic;
using System.Text;
using ConfigMgmt;

namespace AppComponent.ViewModels
{
    public class TodoListVM
    {
        public int Id { get; set; }

        /// <summary>
        /// 源系统
        /// </summary>
        public TODOSYSTEM OrginSystem { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string Module { get; set; }

        /// <summary>
        /// 用户标识（不限于单点登录UserId,IdCardNo)
        /// </summary>
        public string UserIdentity { get; set; }

        /// <summary>
        /// 这个作为用户在SSO里面的Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 代办类型
        /// </summary>
        public string TodoType { get; set; }

        /// <summary>
        /// 代办标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 代办所处的状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 紧急程度
        /// </summary>
        public string Urgent { get; set; }

        /// <summary>
        /// 移动平台接收的时间点
        /// </summary>
        public DateTimeOffset WriteTime { get; set; }

        public AppEntryAuthType AuthType { get; set; }

        /// <summary>
        /// 应用Id 下载使用
        /// </summary>
        public string AppId { get; set; } = "";

        public Dictionary<ClientPlatform, string> OpenUrls { get; set; }
    }

    public enum TODOSYSTEM
    {

        OA = 1,

        PROJECT_MANAGE_SYSTEM = 2,

        KINGDEE_SYSTEM = 3
    }
}
