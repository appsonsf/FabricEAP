using EnterpriseContactService.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnterpriseContact.Entities
{
    public class EmployeePosition
    {
        public Guid EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public Guid PositionId { get; set; }

        public virtual Position Position { get; set; }

        public bool IsPrimary { get; set; }

        public DataSourceType DataSourceType { get; set; }
    }
}
