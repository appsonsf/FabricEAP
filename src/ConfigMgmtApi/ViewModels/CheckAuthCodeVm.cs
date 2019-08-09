using System;

namespace ConfigMgmt.ViewModels
{
    public class CheckAuthCodeVm
    {
        public Guid AppEntranceId { get; set; }

        public string DeviceCode { get; set; }

        public string MobileCode { get; set; }
    }
}