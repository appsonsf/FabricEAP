using System;

namespace ConfigMgmt
{
    /// <summary>
    /// 消息提醒方式
    /// </summary>
    [Flags]
    public enum MsgNotifyApproach
    {
        /// <summary>
        /// 通过TT客户端发送
        /// </summary>
        APP = 1,
        /// <summary>
        /// 通过短信发送
        /// </summary>
        SMS = 2
    }

    /// <summary>
    /// 移动平台客户端平台
    /// </summary>
    [Flags]
    public enum ClientPlatform
    {
        Desktop = 1,
        Android = 2,
        iOS = 4
    }

    /// <summary>
    /// 工作台应用跳转验证方式
    /// </summary>
    [Flags]
    public enum AppEntryAuthType
    {
        /// <summary>
        /// 不验证
        /// </summary>
        None = 0,
        /// <summary>
        /// 临时密码
        /// </summary>
        TempPassword = 1,
        /// <summary>
        /// 访问令牌
        /// </summary>
        AccessToken = 2
    }

    /// <summary>
    /// 工作台应用跳转验证方式
    /// </summary>
    [Flags]
    public enum ComponentDataSourceAuthType
    {
        /// <summary>
        /// 不验证
        /// </summary>
        None = 0,
        /// <summary>
        /// 使用OAuth2.0的ClientCredential验证方式
        /// </summary>
        ClientCredential = 1
    }
}
