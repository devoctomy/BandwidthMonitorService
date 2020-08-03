using System.Diagnostics.CodeAnalysis;

namespace BandwidthMonitorService.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoDownloadUrlsConfiguredException : BandwidthMonitorServiceException
    {
        public NoDownloadUrlsConfiguredException()
            : base("No download Urls have been configured.")
        {

        }
    }
}
