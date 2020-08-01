using System.Threading.Tasks;

namespace BandwidthMonitorService.Services
{
    public interface IPingService
    {
        Task<PingServiceReply> SendPingAsync(string host);
    }
}
