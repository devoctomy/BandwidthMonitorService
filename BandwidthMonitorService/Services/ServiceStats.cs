using BandwidthMonitorService.Dto.Response;
using System;
using System.Threading;

namespace BandwidthMonitorService.Services
{
    public class ServiceStats : IServiceStats
    {
        private int _totalSamplesTaken;
        private long _totalBytesDownloaded;

        public DateTime StartedAt { get; } = DateTime.UtcNow;
        public DateTime StatsLastReset { get; private set; } = DateTime.UtcNow;
        public int TotalSamplesTaken => _totalSamplesTaken;
        public long TotalBytesDownloaded => _totalBytesDownloaded;

        public void Reset()
        {
            _totalSamplesTaken = 0;
            _totalBytesDownloaded = 0;
            StatsLastReset = DateTime.Now;
        }

        public void RegisterSample(Sample sample)
        {
            Interlocked.Add(ref _totalSamplesTaken, 1);
            Interlocked.Add(ref _totalBytesDownloaded, (long)sample.BytesRead); // !!!
        }
    }
}
