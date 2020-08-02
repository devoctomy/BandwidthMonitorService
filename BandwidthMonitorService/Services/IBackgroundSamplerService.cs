using System.Collections.Generic;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Services
{
    public interface IBackgroundSamplerService
    {
        Task Sample();
        List<Dto.Response.Sample> GetSamples();
    }
}
