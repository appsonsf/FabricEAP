using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseContact
{
    public static class FailedCodes
    {
        /// <summary>
        /// 不是群组创建者
        /// </summary>
        public const int Group_NotCreatedBy = 101;
        public const int Group_CannotRemoveOwner = 102;
        public const int Group_OwnerCannotQuit = 103;
    }
}
