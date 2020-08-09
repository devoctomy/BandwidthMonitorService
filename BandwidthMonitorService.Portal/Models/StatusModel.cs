using BandwidthMonitorService.Dto.Response;

namespace BandwidthMonitorService.Portal.Models
{
    public class StatusModel
    {
        public bool IsOnline { get; set; }
        public ServiceStatus ServiceStatus { get; set; }
    }
}
