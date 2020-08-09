using BandwidthMonitorService.Client.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Portal.Messages
{
    public class GetServiceStatusHandler : IRequestHandler<GetServiceStatusQuery, GetServiceStatusResponse>
    {
        private readonly IStatusClient _statusClient;

        public GetServiceStatusHandler(IStatusClient statusClient)
        {
            _statusClient = statusClient;
        }

        public async Task<GetServiceStatusResponse> Handle(
            GetServiceStatusQuery request,
            CancellationToken cancellationToken)
        {
            var response = await _statusClient.GetServiceStatusAsync(cancellationToken);
            bool isOnline = response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            return new GetServiceStatusResponse()
            {
                IsOnline = isOnline,
                ServiceStatus = isOnline ? response.Value : null
            };
        }
    }
}
