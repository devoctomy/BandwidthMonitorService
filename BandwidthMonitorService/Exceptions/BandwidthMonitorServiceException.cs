using System;

namespace BandwidthMonitorService.Exceptions
{
    public class BandwidthMonitorServiceException : Exception
    {
        public BandwidthMonitorServiceException(string message)
            : base(message)
        { }

        public BandwidthMonitorServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
