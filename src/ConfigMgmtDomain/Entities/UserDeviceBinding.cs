using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ConfigMgmt.Entities
{
    /// <summary>
    /// 用户设备绑定聚合
    /// </summary>
    [DataContract]
    public class UserDeviceBinding
    {
        /// <summary>
        /// UserId
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Dictionary<string, DeviceBindingInfo> Devices { get; set; }

        public static UserDeviceBinding Create(Guid userId, string deviceCode, Guid appEntranceId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentOutOfRangeException("must not be empty", nameof(userId));

            if (string.IsNullOrEmpty(deviceCode))
                throw new ArgumentNullException(nameof(deviceCode));

            if (appEntranceId == Guid.Empty)
                throw new ArgumentOutOfRangeException("must not be empty", nameof(appEntranceId));

            return new UserDeviceBinding
            {
                Id = userId,
                Devices = new Dictionary<string, DeviceBindingInfo>
                {
                    [deviceCode] = DeviceBindingInfo.Create(appEntranceId)
                }
            };
        }

        public void AddOrUpdateLatestAuthed(string deviceCode, Guid appEntranceId)
        {
            if (string.IsNullOrEmpty(deviceCode))
                throw new ArgumentNullException(nameof(deviceCode));

            if (appEntranceId == Guid.Empty)
                throw new ArgumentOutOfRangeException("must not be empty", nameof(appEntranceId));

            if (Devices.ContainsKey(deviceCode))
            {
                var info = Devices[deviceCode];
                if (info.AppEntranceAuths.ContainsKey(appEntranceId))
                {
                    var auth = info.AppEntranceAuths[appEntranceId];
                    auth.LatestAuthed = DateTimeOffset.UtcNow;
                }
                else
                    info.AppEntranceAuths[appEntranceId] = AppEntranceAuth.Create();
            }
            else
                Devices[deviceCode] = DeviceBindingInfo.Create(appEntranceId);
        }

        public bool CheckWhetherAuth(string deviceCode, Guid appEntranceId, TimeSpan validity)
        {
            if (string.IsNullOrEmpty(deviceCode))
                throw new ArgumentNullException(nameof(deviceCode));

            if (appEntranceId == Guid.Empty)
                throw new ArgumentOutOfRangeException("must not be empty", nameof(appEntranceId));

            if (Devices?.ContainsKey(deviceCode) == true)
            {
                var info = Devices[deviceCode];
                if (info.AppEntranceAuths?.ContainsKey(appEntranceId) == true)
                {
                    var auth = info.AppEntranceAuths[appEntranceId];
                    return (DateTimeOffset.UtcNow - auth.LatestAuthed) > validity;//最后验证时间超过有效期
                }
                else
                    return true;
            }
            else
                return true;
        }
    }


    /// <summary>
    /// 设备绑定信息
    /// </summary>
    [DataContract]
    public class DeviceBindingInfo
    {
        [DataMember]
        public Dictionary<Guid, AppEntranceAuth> AppEntranceAuths { get; set; }

        internal static DeviceBindingInfo Create(Guid appEntranceId)
        {
            return new DeviceBindingInfo
            {
                AppEntranceAuths = new Dictionary<Guid, AppEntranceAuth>
                {
                    [appEntranceId] = AppEntranceAuth.Create()
                }
            };
        }
    }

    [DataContract]
    public class AppEntranceAuth
    {
        [DataMember]
        public DateTimeOffset LatestAuthed { get; set; }

        internal static AppEntranceAuth Create()
        {
            return new AppEntranceAuth { LatestAuthed = DateTimeOffset.UtcNow };
        }
    }
}
