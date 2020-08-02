using BandwidthMonitorService.Dto.Response;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Services
{
    public interface ISamplerService
    {
        public Task<Sample> Sample(
            string sampleUrl,
            CancellationToken cancellationToken);
    }
}
