using ConfigMgmt.Entities;
using System;
using System.Collections.Generic;

namespace ConfigMgmt
{
    public class WorkbenchAppServiceInput
    {
        public string ClientId { get; set; }
    }

    public class GetAppEntrancesInput : WorkbenchAppServiceInput
    {
        public ClientPlatform ClientPlatform { get; set; }

        public List<string> UserRoles { get; set; }

        //public Guid UserId { get; set; }
    }

    public class AddEntrancesInput : WorkbenchAppServiceInput
    {
        public List<AppEntrance> AppEntrances { get; set; }
    }

    public class AppEntranceAuthStateInput
    {
        public Guid AppEntranceId { get; set; }
        public string DeviceCode { get; set; }
    }
}
