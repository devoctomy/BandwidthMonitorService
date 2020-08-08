using BandwidthMonitorService.Dto.Response;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Client.Services
{
    [BandwidthMonitorServiceClient(
        UniqueName = "StatusClient",
        Implementation = typeof(StatusClient))]
    public interface IStatusClient
    {
        public Task<Response<ServiceStatus>> GetServiceStatusAsync(CancellationToken cancellationToken);
    }
}
