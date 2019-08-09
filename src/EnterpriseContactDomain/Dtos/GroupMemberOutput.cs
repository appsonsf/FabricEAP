using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseContact
{
    [Serializable]
    public class GroupMemberOutput
    {
        public Guid EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public int Gender { get; set; }

        public bool IsOwner { get; set; }
        
        public Guid PrimaryDepartmentId { get; set; }

        public DateTimeOffset Joined { get; set; }
    }
}
