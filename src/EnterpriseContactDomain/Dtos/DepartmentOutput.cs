using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseContact
{
    [Serializable]
    public class DepartmentOutput
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public string Number { get; set; }

        public string Name { get; set; }

        public int TypeId { get; set; }

        public int Sort { get; set; }

        public int? IconId { get; set; }
    }
}
