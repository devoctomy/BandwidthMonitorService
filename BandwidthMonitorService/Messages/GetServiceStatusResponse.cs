using BandwidthMonitorService.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Messages
{
    public class GetServiceStatusResponse
    {
        public ServiceStatus ServiceStatus { get; set; }
    }
}
