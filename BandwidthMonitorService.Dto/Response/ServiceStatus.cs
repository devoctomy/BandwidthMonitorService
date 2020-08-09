using System;
using System.ComponentModel;

namespace BandwidthMonitorService.Dto.Response
{
    public class ServiceStatus
    {
        public TimeSpan Uptime { get; set; }

        [DisplayName("Started @")]
        public DateTime StartedAt { get; set; }

        [DisplayName("Stats Last Reset @")]
        public DateTime StatsLastReset { get; set; }

        [DisplayName("Total Samples Taken")]
        public int TotalSamplesTaken { get; set; }

        [DisplayName("Total Bytes Downloaded")]
        public long TotalBytesDownloaded { get; set; }
    }
}
