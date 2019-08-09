using EnterpriseContactService.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseContact.Entities
{
    public class Position
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        public Guid DepartmentId { get; set; }

        public DataSourceType DataSourceType { get; set; }

        public virtual Department Department { get; set; }
    }
}
