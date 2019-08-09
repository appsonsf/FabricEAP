using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseContact.MsgConstracts
{
    public abstract class BaseDepartmentEvent
    {
        public Guid Id { get; set; }
    }

    public class DepartmentDeleted : BaseDepartmentEvent
    { }

    public class DepartmentMembersUpdated : BaseDepartmentEvent
    { }
}
