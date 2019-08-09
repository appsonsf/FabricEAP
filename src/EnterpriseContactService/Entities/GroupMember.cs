using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseContact.Entities
{
    public class GroupMember
    {
        public Guid GroupId { get; set; }

        public virtual Group Group { get; set; }

        public Guid EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public bool IsOwner { get; set; }

        /// <summary>
        /// 加入时间
        /// </summary>
        public DateTimeOffset Joined { get; set; }
    }
}
