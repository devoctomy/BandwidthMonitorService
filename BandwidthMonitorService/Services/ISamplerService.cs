using BandwidthMonitorService.Dto.Response;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Services
{
    public interface ISamplerService
    {
        public Task<SamplerServiceResult> Sample(
            string sampleUrl,
            CancellationToken cancellationToken);
    }
}
