using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseContact.ViewModels
{
    public class GroupDetailVm : GroupListVm
    {
        public string Remark { get; set; }

        public List<GroupMemberListVm> Members { get; set; }
    }
}
