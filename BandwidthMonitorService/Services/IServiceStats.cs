using System;

namespace BandwidthMonitorService.Services
{
    public interface IServiceStats
    {
        public DateTime StartedAt { get; }
        public DateTime StatsLastReset { get; }
        public int TotalSamplesTaken { get; }
        public long TotalBytesDownloaded { get; }

        void Reset();

        void RegisterSample(long totalBytes);
    }
}
