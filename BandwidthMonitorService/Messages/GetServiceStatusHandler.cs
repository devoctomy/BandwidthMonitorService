using BandwidthMonitorService.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Messages
{
    public class GetServiceStatusHandler : IRequestHandler<GetServiceStatusQuery, GetServiceStatusResponse>
    {
        private readonly ILogger<GetServiceStatusHandler> _logger;
        private readonly IServiceStats _serviceStats;

        public GetServiceStatusHandler(
            ILogger<GetServiceStatusHandler> logger,
            IServiceStats serviceStats)
        {
            _logger = logger;
            _serviceStats = serviceStats;
        }

        public async Task<GetServiceStatusResponse> Handle(
            GetServiceStatusQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Yield();

            return new GetServiceStatusResponse()
            {
                ServiceStatus = new Dto.Response.ServiceStatus()
                {
                    Uptime = DateTime.UtcNow.Subtract(_serviceStats.StartedAt),
                    StartedAt = _serviceStats.StartedAt,
                    StatsLastReset = _serviceStats.StatsLastReset,
                    TotalBytesDownloaded = _serviceStats.TotalBytesDownloaded,
                    TotalSamplesTaken = _serviceStats.TotalSamplesTaken
                }
            };
        }
    }
}
