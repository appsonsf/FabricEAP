using Notification;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseContact.MsgConstracts
{
    public abstract class BaseGroupEvent
    {
        public Guid Id { get; set; }
    }

    public class GroupAdded : BaseGroupEvent
    {
    }

    public class GroupMembersUpdated : BaseGroupEvent
    {
    }

    public class GroupNotified : BaseGroupEvent
    {
        public List<EventNotifyDto> Notifies { get; set; }
    }

    public class GroupDeleted : BaseGroupEvent
    {
    }
}
