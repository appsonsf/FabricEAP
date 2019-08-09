using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseContact
{
    [Serializable]
    public class EmployeeListOutput
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Gender { get; set; }

        public string Avatar { get; set; }

        public Guid PrimaryDepartmentId { get; set; }

        public Guid PrimaryPositionId { get; set; }

        public string PositionName { get; set; }

        public Guid? PositionId { get; set; }
    }
}
