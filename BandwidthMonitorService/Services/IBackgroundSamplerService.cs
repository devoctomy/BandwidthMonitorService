using System.Collections.Generic;

namespace BandwidthMonitorService.Services
{
    public interface IBackgroundSamplerService
    {
        List<Dto.Response.Sample> GetSamples();
    }
}
