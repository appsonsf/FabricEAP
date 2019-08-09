using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseContact
{
    public class GroupListOutput
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public GroupType Type { get; set; }

        public int? IconId { get; set; }
    }
}
