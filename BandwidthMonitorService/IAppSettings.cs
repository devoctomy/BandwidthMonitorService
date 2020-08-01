namespace BandwidthMonitorService
{
    public interface IAppSettings
    {
        int DownloadBufferSize { get; set; }
        int MinutesBetweenSamples { get; set; }
        string DownloadUrlFrankfurt { get; set; }
        string DownloadUrlIreland { get; set; }
        string DownloadUrlLondon { get; set; }
        string DownloadUrlParis { get; set; }
        int SecondsDelayBeforeFirstSample { get; set; }
    }
}
