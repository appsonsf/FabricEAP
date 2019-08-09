using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseContact
{
    [Serializable]
    public class DepartmentListOutput
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public string Name { get; set; }

        public int? IconId { get; set; }
    }
}
