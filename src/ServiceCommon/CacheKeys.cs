using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class CacheKeys
    {
        /// <summary>
        /// 15 min
        /// </summary>
        public static readonly TimeSpan DefaultCacheAbsoluteExpiration = TimeSpan.FromMinutes(15);

        public const string EnterpriseContact_GetRoots = "EnterpriseContact.GetRoots";
        public const string EnterpriseContact_DepartmentList = "EnterpriseContact.DepartmentList";
        public const string EnterpriseContact_PositionList = "EnterpriseContact.PositionList";
        public const string Common_EmployeeList = "Common.EmployeeList";
    }
}
