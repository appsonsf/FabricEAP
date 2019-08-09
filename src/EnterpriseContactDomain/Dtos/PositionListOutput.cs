using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseContact
{
    [Serializable]
    public class PositionListOutput
    {
        public Guid Id { get; set; }

        public Guid DepartmentId { get; set; }

        public string Name { get; set; }
    }
}
