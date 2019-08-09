using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseContact.ViewModels
{
    public class OrgVm
    {
        public List<DepartmentListVm> Departments { get; set; }

        public List<EmployeeListVm> Employees { get; set; }
    }
}
