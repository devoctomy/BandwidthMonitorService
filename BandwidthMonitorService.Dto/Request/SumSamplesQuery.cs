using System;

namespace BandwidthMonitorService.Dto.Request
{
    public class SumSamplesQuery
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
