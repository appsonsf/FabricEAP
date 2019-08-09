using System;

namespace EnterpriseContact
{
    public enum GroupType
    {
        /// <summary>
        /// 白名单聊天组，用于控制IM对点对免打扰的功能
        /// </summary>
        WhiteListChat = 1,
        /// <summary>
        /// 部门聊天组，系统自动维护的组，根据部门包含员工自动改变成员
        /// 部门组为虚拟的，不会产生真实数据
        /// </summary>
        DepartmentChat = 2,
        /// <summary>
        /// 自定义聊天组，由用户创建和手动维护的组
        /// </summary>
        CustomChat = 3,
    }
}