using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseContact.ViewModels
{
    public class EmployeeListVm
    {
        public Guid Id { get; set; }
      
        public string Name { get; set; }

        public int Gender { get; set; }

        public string PositionName { get; set; }

        public bool IsPrimary { get; set; }

        /// <summary>
        /// 员工所属主部门的名称，及其上级名称，Controller可以选择填充几级
        /// </summary>
        public List<string> DepartmentNames { get; set; }
    }
}
