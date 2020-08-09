﻿namespace BandwidthMonitorService
{
    public interface IAppSettings
    {
        public string MongoDbConnectionString { get; set; }
        public bool StoreSamples { get; set; }
        int DownloadBufferSize { get; set; }
        int MinutesBetweenSamples { get; set; }
        int SecondsDelayBeforeFirstSample { get; set; }
        string DownloadSampleUrlsFile { get; set; }
    }
}
