using System.Net.NetworkInformation;

namespace BandwidthMonitorService.Services
{
    public class PingServiceReply
    {
        public long RoundTripTime { get; set; }
        public IPStatus Status { get; set; }
    }
}
