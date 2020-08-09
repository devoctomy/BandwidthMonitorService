namespace BandwidthMonitorService
{
    public class AppSettings : IAppSettings
    {
        public string MongoDbConnectionString { get; set; }
        public bool StoreSamples { get; set; }
        public int DownloadBufferSize { get; set; }
        public int MinutesBetweenSamples { get; set; }
        public int SecondsDelayBeforeFirstSample { get; set; }
        public string DownloadSampleUrlsFile { get; set; }
    }
}
