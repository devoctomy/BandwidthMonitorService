namespace BandwidthMonitorService.Exceptions
{
    public class NoDownloadUrlsConfiguredException : BandwidthMonitorServiceException
    {
        public NoDownloadUrlsConfiguredException()
            : base("No download Urls have been configured.")
        {

        }
    }
}
