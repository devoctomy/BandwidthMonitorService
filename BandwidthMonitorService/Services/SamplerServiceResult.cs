using BandwidthMonitorService.Dto.Response;
using System;

namespace BandwidthMonitorService.Services
{
    public class SamplerServiceResult
    {
        public bool IsSuccess { get; set; }
        public Sample Sample { get; set; }
        public Exception Exception { get; set; }
    }
}
