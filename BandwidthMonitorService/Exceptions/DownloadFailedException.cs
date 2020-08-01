namespace BandwidthMonitorService.Exceptions
{
    public class DownloadFailedException : BandwidthMonitorServiceException
    {
        public string Url { get; }

        public DownloadFailedException(string url)
            : base($"Failed to download {url}.")
        {
            Url = url;
        }
    }
}
