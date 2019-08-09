using Common;
using MassTransit;
using MassTransit.AspNetCoreIntegration;
using AppsOnSF.Common.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceFabricContrib;
using System;
using System.Fabric;


namespace NotifyGatewayWeb.EventBus
{
    public static class EventBusExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<SendEventNotifyConsumer>();
                x.AddConsumer<SendMessageNotifyConsumer>();

                x.AddBus(provider =>
                {
                    return Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        var serviceContext = provider.GetRequiredService<StatelessServiceContext>();
                        var option = serviceContext.GetOption<RabbitMQOption>("RabbitMQ_ms");

                        var host = cfg.Host(new Uri(option.HostAddress), h =>
                        {
                            h.Username(option.Username);
                            h.Password(option.Password);
                        });

                        cfg.ReceiveEndpoint(host, RabbitMqReceiveEndpointNames.NotifyGateway, e =>
                        {
                            e.ConfigureConsumer<SendEventNotifyConsumer>(provider);
                            e.ConfigureConsumer<SendMessageNotifyConsumer>(provider);
                        });
                    });
                });
            });

            services.AddSingleton<IHostedService>(provider =>
            {
                var buses = provider.GetServices<IBusControl>();
                return new BusHostedService(buses);
            });

            return services;
        }
    }
}
