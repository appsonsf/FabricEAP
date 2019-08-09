using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class Constants
    {
#if DEBUG
        public const string AppName_AppComponent = "Eap";
        public const string AppName_Gateway = "Eap";
        public const string AppName_Attachment = "Eap";
        public const string AppName_ConfigMgmt = "Eap";
        public const string AppName_EnterpriseContact = "Eap";
        public const string AppName_GroupFile = "Eap";
        public const string AppName_InstantMessage = "Eap";
        public const string AppName_Notify = "Eap";
#else
        public const string AppName_AppComponent = "EapAc";
        public const string AppName_Gateway = "EapGateway";
        public const string AppName_Attachment = "EapAttach";
        public const string AppName_ConfigMgmt = "EapCm";
        public const string AppName_EnterpriseContact = "EapEc";
        public const string AppName_GroupFile = "EapGf";
        public const string AppName_InstantMessage = "EapIm";
        public const string AppName_Notify = "EapNotify";
#endif
        public const string ServiceName_AttachmentStateService = "AttachmentStateService";
        public const string ServiceName_UserConfigStateService = "UserConfigStateService";
        public const string ServiceName_SystemConfigStateService = "SystemConfigStateService";
        public const string ServiceName_EnterpriseContactService = "EnterpriseContactService";
        public const string ServiceName_GroupFileService = "GroupFileService";
        public const string ServiceName_ConversationCtrlStateService = "ConversationCtrlStateService";
        public const string ServiceName_ConversationMsgStateService = "ConversationMsgStateService";
        public const string ServiceName_NotifySessionActorService = "NotifySessionActorService";

#region ListenerName_UserConfigStateService
        public const string ListenerName_UserSettingAppService = "UserSettingAppService";
        public const string ListenerName_UserFavoriteAppService = "UserFavoriteAppService";
#endregion

#region ListenerName_SystemConfigStateService
        public const string ListenerName_ClientAppService = "ClientAppService";
        public const string ListenerName_WorkbenchAppService = "WorkbenchAppService";
        public const string ListenerName_TodoCenterAppService = "TodoCenterAppService";
#endregion

#region ListenerName_EnterpriseContactService
        public const string ListenerName_DepartmentAppService = "DepartmentAppService";
        public const string ListenerName_EmployeeAppService = "EmployeeAppService";
        public const string ListenerName_PositionAppService = "PositionAppService";
        public const string ListenerName_GroupAppService = "GroupAppService";
#endregion

#region ListenerName_GroupFileService
        public const string ListenerName_ControlAppService = "ControlAppService";
        #endregion

#if DEBUG
        public const string DnsName_TodoCenterProxyApi = "http://localhost:10123";
#else
        public const string DnsName_TodoCenterProxyApi = "http://todocenterproxyapi.eap:10123";
#endif
        //TODO 待办中心的配置应该加入 http://todocenterproxyapi.eap:10123
    }
}
