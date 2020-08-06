using System;

namespace BandwidthMonitorService.Dto.Request
{
    public class GetSummedGraphDataQuery
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
