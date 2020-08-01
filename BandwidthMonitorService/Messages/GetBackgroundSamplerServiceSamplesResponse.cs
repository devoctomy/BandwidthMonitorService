using System.Collections.Generic;

namespace BandwidthMonitorService.Messages
{
    public class GetBackgroundSamplerServiceSamplesResponse
    {
        public IList<Dto.Response.Sample> Samples { get; set; }
    }
}
