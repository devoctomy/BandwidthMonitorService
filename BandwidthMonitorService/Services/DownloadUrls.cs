using System.Collections.Generic;

namespace BandwidthMonitorService.Services
{
    public class DownloadUrls : List<string>
    {
        public DownloadUrls()
        { }

        public DownloadUrls(List<string> urls)
            : base(urls)
        { }
    }
}
