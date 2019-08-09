using MassTransit;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NotifyGatewayWeb.EventBus
{
    public class BusHostedService : IHostedService, IDisposable
    {
        private readonly IEnumerable<IBusControl> _buses;

        public BusHostedService(IEnumerable<IBusControl> buses)
        {
            _buses = buses;
        }

        public void Dispose()
        {
            if (_buses != null)
            {
                _buses.ForEach(o => o.Stop());
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_buses != null)
            {
                var tasks = _buses.Select(o =>
                {
                    ServiceEventSource.Current.Message("EventBus Started: " + o.Address);
                    return o.StartAsync();
                });
                return Task.WhenAll(tasks);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_buses != null)
            {
                var tasks = _buses.Select(o =>
                {
                    ServiceEventSource.Current.Message("EventBus Stoped: " + o.Address);
                    return o.StopAsync();
                });
                return Task.WhenAll(tasks);
            }
            return Task.CompletedTask;
        }
    }
}
