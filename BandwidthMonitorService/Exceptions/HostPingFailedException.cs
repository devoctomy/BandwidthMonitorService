using System.Net.NetworkInformation;

namespace BandwidthMonitorService.Exceptions
{
    public class HostPingFailedException : BandwidthMonitorServiceException
    {
        public string Host { get; }
        public IPStatus Status { get; }

        public HostPingFailedException(string host, IPStatus status)
            : base($"Ping failed to host '{host}', status = {status}.")
        {
            Host = host;
            Status = status;
        }
    }
}
