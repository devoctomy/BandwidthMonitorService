using System;

namespace BandwidthMonitorService.Dto.Response
{
    public class ServiceStatus
    {
        public TimeSpan Uptime { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime StatsLastReset { get; set; }
        public int TotalSamplesTaken { get; set; }
        public long TotalBytesDownloaded { get; set; }
    }
}
