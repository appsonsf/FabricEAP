using AppsOnSF.Common.BaseServices;
using Base.Csi.Sms.MsgContracts;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public class MobileCodeSender : IMobileCodeSender
    {
        private const string SimpleKeyValueServiceContainerName = "Eap.MobileCode";

        private readonly IBusControl _bus;
        private readonly ISimpleKeyValueService _simpleKeyValueService;

        public MobileCodeSender(IBusControl bus, ISimpleKeyValueService simpleKeyValueService)
        {
            _bus = bus;
            _simpleKeyValueService = simpleKeyValueService;
        }

        public async Task<bool> CheckAsync(string phoneNumber, string code)
        {
            var original = await _simpleKeyValueService.CheckAndGet(
                   SimpleKeyValueServiceContainerName, phoneNumber, TimeSpan.FromMinutes(5));
            return original == code;
        }

        public async Task<string> SendAsync(string phoneNumber)
        {
            var code = (new Random().Next(1000, 9999)).ToString();

            await _simpleKeyValueService.AddOrUpdate(
                     SimpleKeyValueServiceContainerName, phoneNumber, code);

            await _bus.Send<SendMobileCodeCommand>(new
            {
                PhoneNumbers = new string[] { phoneNumber },
                Code = code,
            });

            return code;
        }
    }
}
