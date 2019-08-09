using System;

namespace EnterpriseContact.ViewModels
{
    /// <summary>
    /// 部门基本信息
    /// </summary>
    public class DepartmentListVm
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public int? IconId { get; set; }
    }

    public class DepartmentSearchListVm : DepartmentListVm
    {
        public string ParentName { get; set; }
    }
}
