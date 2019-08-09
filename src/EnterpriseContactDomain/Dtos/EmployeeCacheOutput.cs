using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseContact
{
    public class EmployeeCacheOutput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public Guid? UserId { get; set; }
        public string Mobile { get; set; }
        public int Gender { get; set; }
        //public string Avatar { get; set; }
    }
}
