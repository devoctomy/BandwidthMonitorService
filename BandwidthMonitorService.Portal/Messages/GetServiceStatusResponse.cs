using BandwidthMonitorService.Dto.Response;

namespace BandwidthMonitorService.Portal.Messages
{
    public class GetServiceStatusResponse
    {
        public bool IsOnline { get; set; }
        public ServiceStatus ServiceStatus { get; set; }
    }
}
