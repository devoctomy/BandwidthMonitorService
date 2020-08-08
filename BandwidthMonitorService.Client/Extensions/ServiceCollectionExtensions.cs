using BandwidthMonitorService.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace BandwidthMonitorService.Client.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBandwidthMonitorClients(
            this IServiceCollection services,
            BandwidthMonitorClientSettings settings)
        {
            var assembly = typeof(ServiceCollectionExtensions).Assembly;
            var clientTypes = assembly.GetTypes().Where(x => 
                x.GetCustomAttribute<BandwidthMonitorServiceClientAttribute>() != null)
                .ToList();
            foreach (var curClientType in clientTypes)
            {
                var clientAttribute = curClientType.GetCustomAttribute<BandwidthMonitorServiceClientAttribute>();
                services.AddHttpClient(clientAttribute.UniqueName, c =>
                {
                    c.BaseAddress = new Uri(settings.BaseUrl);
                    c.DefaultRequestHeaders.Add("Accept", "application/json");
                    c.DefaultRequestHeaders.Add("User-Agent", "BandwidthMonitorService.Client");
                });

                services.AddSingleton(curClientType, clientAttribute.Implementation);
            }
        }
    }
}
