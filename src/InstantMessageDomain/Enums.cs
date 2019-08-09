using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace InstantMessage
{
    [DataContract]
    public enum ConversationType
    {
        [EnumMember]
        P2P = 1,
        //Group = 2,
        [EnumMember(Value = "Group")]
        CustomGroup = 2,
        [EnumMember]
        DepartmentGroup = 3
    }

    public enum ConversationMsgType
    {
        Text = 1,
        Image = 2,
        File = 3,
        Voice = 4,
        Video = 5,
        Location = 6
    }
}
