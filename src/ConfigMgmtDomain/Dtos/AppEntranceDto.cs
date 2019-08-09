using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigMgmt
{
    /// <summary>
    /// 工作台中的应用入口信息
    /// </summary>
    public class AppEntranceDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }

        /// <summary>
        /// 关联文件夹Id
        /// </summary>
        public Guid? FolderId { get; set; }

        /// <summary>
        /// 是否是文件夹的配置
        /// </summary>
        public bool IsFolder { get; set; }

        /// <summary>
        /// 图标显示此次比例，1=1x，2=高2x，3=宽2x，4=高宽2x
        /// </summary>
        public int IconDisplaySize { get; set; }

        public int Order { get; set; }

        public AppEntryAuthType AuthType { get; set; }

        public string[] NeedRoles { get; set; }

        /// <summary>
        /// 入口跳转Uri
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// 应用id
        /// </summary>
        public string AppId { get; set; } = "";

        #region component info
        public bool ShowBadge { get; set; }
        #endregion

        public bool NeedAuth { get; set; }
    }
}
