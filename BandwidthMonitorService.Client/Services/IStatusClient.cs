using BandwidthMonitorService.Dto.Response;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Client.Services
{
    public interface IStatusClient
    {
        public Task<Response<ServiceStatus>> GetServiceStatusAsync(CancellationToken cancellationToken);
    }
}
