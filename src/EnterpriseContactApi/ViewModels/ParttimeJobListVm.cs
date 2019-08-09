using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseContact.ViewModels
{
    public class ParttimeJobListVm
    {
        //public Guid DepartmentId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public List<string> DepartmentNames { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string PositionName { get; set; }
    }
}
