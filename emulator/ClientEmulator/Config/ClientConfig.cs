using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientEmulator.Config
{
    public static class ClientConfig
    {
        public static string ClientId { get; set; }

        public static string ClientSercet { get; set; }

        public static string[] Scopes { get; set; }

        static ClientConfig()
        {
            ClientId = "OM_IM_Desktop_001";
            ClientSercet = "OMIMDesktop001";
            //openid profile profile.ext sso.sts eap.api
            Scopes = new[] { "openid", "sso.sts", "mp.apigateway", "profile", "profile.ext", "eap.api" };
        }
    }
}
