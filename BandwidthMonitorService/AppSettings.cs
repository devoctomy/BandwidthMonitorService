namespace BandwidthMonitorService
{
    public class AppSettings : IAppSettings
    {
        public bool SaveSamples { get; set; }
        public int DownloadBufferSize { get; set; }
        public int MinutesBetweenSamples { get; set; }
        public string DownloadUrlFrankfurt { get; set; }
        public string DownloadUrlIreland { get; set; }
        public string DownloadUrlLondon { get; set; }
        public string DownloadUrlParis { get; set; }
        public int SecondsDelayBeforeFirstSample { get; set; }
    }
}
