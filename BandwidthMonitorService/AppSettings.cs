namespace BandwidthMonitorService
{
    public class AppSettings : IAppSettings
    {
        public bool SaveSamples { get; set; }
        public int DownloadBufferSize { get; set; }
        public int MinutesBetweenSamples { get; set; }
        public int SecondsDelayBeforeFirstSample { get; set; }
        public string DownloadSampleUrlsFile { get; set; }
    }
}
