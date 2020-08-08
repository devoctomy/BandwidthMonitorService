using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Messages
{
    public class GetServiceStatusHandler : IRequestHandler<GetServiceStatusQuery, GetServiceStatusResponse>
    {
        private ILogger<GetServiceStatusHandler> _logger;

        public GetServiceStatusHandler(ILogger<GetServiceStatusHandler> logger)
        {
            _logger = logger;
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
                    Uptime = new TimeSpan(1, 0, 0)
                }
            };
        }
    }
}
