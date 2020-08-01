using System;

namespace BandwidthMonitorService.Dto.Response
{
    public class Sample
    {
        public string Id { get; set; }
        public int Timestamp { get; set; }
        public string Url { get; set; }
        public long BytesRead { get; set; }
        public long TotalReads { get; set; }
        public TimeSpan Elapsed { get; set; }
        public long RoundTripTime { get; set; }
    }
}
