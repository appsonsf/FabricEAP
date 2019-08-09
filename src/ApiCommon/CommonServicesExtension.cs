using AppsOnSF.Common.BaseServices;
using Base.Csi.Sms.MsgContracts;
using Common.Services;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class CommonServicesExtension
    {
        public const string RabbitMqReceiveEndpointNames_SmsService = "Base.Csi.SmsService";

        public static IServiceCollection AddCommonServices(this IServiceCollection services, IBusControl bus, string busHost)
        {
            services.AddSingleton<IMobileCodeSender>(_ => new MobileCodeSender(
                bus, RemotingProxyFactory.CreateSimpleKeyValueService()));

            EndpointConvention.Map<SendMobileCodeCommand>(
                new Uri(busHost + RabbitMqReceiveEndpointNames_SmsService));

            return services;
        }
    }
}
