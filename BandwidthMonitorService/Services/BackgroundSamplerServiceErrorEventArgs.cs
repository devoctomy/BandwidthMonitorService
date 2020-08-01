using System;

namespace BandwidthMonitorService.Services
{
    public class BackgroundSamplerServiceErrorEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
    }
}
