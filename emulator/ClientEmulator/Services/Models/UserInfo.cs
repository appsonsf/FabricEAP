using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientEmulator.Services.Models
{
    public class UserInfo
    {
        /// <summary>
        /// SSO ID,type is guid
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// MDMID,type is guid
        /// </summary>
        public Guid UserMdmId { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string AccessToken { get; set; }

        public static List<UserInfo> LoginedUsers { get; set; } = new List<UserInfo>();
    }
}
