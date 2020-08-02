using System;

namespace BandwidthMonitorService.Dto.Response
{
    public class Sample
    {
        public string Id { get; set; }
        public int Timestamp { get; set; }
        public string Url { get; set; }
        public double BytesRead { get; set; }
        public double TotalReads { get; set; }
        public TimeSpan Elapsed { get; set; }
        public double RoundTripTime { get; set; }
    }
}
