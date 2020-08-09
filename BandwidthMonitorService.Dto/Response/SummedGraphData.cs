using System.Collections.Generic;

namespace BandwidthMonitorService.Dto.Response
{
    public class SummedGraphData
    {
        public Dto.Response.Summary Summary { get; set; }
        public List<Dto.Response.Sample> Samples { get; set; }
    }
}
