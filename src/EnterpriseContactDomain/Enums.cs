using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseContact
{
    [Flags]
    public enum GroupEventNotifyType
    {
        ParticipantAdded = 1,
        ParticipantRemoved = 2,
        GroupDismissed = 3,
        ParticipantQuited = 4,
        /// <summary>
        /// 群组名称修改
        /// </summary>
        GroupNameUpdated = 5,
    }
}
