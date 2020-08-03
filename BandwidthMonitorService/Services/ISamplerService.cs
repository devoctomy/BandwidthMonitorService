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
            bool store,
            CancellationToken cancellationToken);

        public Task<SamplerServiceResult> Sample(
            string sampleUrl,
            bool store,
            CancellationToken cancellationToken);
    }
}
