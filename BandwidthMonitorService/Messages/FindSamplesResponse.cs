using System.Collections.Generic;

namespace BandwidthMonitorService.Messages
{
    public class FindSamplesResponse
    {
        public List<Dto.Response.Sample> Samples { get; set; }
    }
}
