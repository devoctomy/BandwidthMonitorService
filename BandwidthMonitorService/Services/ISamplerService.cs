using BandwidthMonitorService.Dto.Response;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Services
{
    public interface ISamplerService
    {
        public Task<List<SamplerServiceResult>> Sample(
            List<string> sampleUrls,
            CancellationToken cancellationToken);

        public Task<SamplerServiceResult> Sample(
            string sampleUrl,
            CancellationToken cancellationToken);
    }
}
