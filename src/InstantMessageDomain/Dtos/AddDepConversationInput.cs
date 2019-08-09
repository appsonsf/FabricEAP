using System;
using System.Collections.Generic;
using System.Text;

namespace InstantMessage
{
    [Serializable]
    public class AddDepConversationInput
    {
        public Guid UserId { get; set; }

        public Guid DepartmentId { get; set; }
    }
}
