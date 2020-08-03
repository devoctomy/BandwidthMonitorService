using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Services
{
    public interface IBackgroundSamplerService
    {
        Task CollectAsync(CancellationToken cancellationToken);
        List<Dto.Response.Sample> GetSamples();
    }
}
