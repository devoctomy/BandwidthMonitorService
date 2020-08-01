using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Services
{
    public class PingService : IPingService
    {
        private Ping _ping;

        public PingService(Ping ping)
        {
            _ping = ping;
        }

        public async Task<PingServiceReply> SendPingAsync(string host)
        {
            var reply = await _ping.SendPingAsync(host);
            return new PingServiceReply()
            {
                Status = reply.Status,
                RoundTripTime = reply.RoundtripTime
            };
        }
    }
}
