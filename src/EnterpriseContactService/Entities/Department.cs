using EnterpriseContactService.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseContact.Entities
{
    public class Department
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public virtual Department Parent { get; set; }

        //[Index]
        [Required]
        [StringLength(128)]
        public string Number { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        public int TypeId { get; set; }

        public int Sort { get; set; }

        public int? IconId { get; set; }

        public DataSourceType DataSourceType { get; set; }

        public virtual List<Position> Positions { get; set; }
    }
}
