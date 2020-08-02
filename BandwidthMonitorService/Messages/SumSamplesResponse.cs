using System.Collections.Generic;

namespace BandwidthMonitorService.Messages
{
    public class SumSamplesResponse
    {
        public List<Dto.Response.Sample> Samples { get; set; }
    }
}
