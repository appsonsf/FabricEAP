using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseContact.ViewModels
{
    public class GroupListVm
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int? IconId { get; set; }

        public Guid CreatedBy { get; set; }

        /// <summary>
        /// 是否部门群
        /// </summary>
        public bool IsDepartmentGroup { get; set; }
    }
}
