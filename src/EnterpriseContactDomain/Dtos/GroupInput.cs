using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseContact
{
    [Serializable]
    public class GroupInput
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// 联系人组的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 联系人组的备注信息
        /// </summary>
        public string Remark { get; set; }

        public Guid CurrentUserId { get; set; }

        public Guid? CurrentEmployeeId { get; set; }

        public string CurrentEmployeeName { get; set; }

        public HashSet<Guid> AddingMemberIds { get; set; }

        public List<string> AddingMemberNames { get; set; }

        public HashSet<Guid> RemovingMemberIds { get; set; }

        public List<string> RemovingMemberNames { get; set; }
    }
}
