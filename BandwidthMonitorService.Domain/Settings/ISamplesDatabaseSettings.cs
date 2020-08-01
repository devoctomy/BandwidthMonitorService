namespace BandwidthMonitorService.Domain.Settings
{
    public interface ISamplesDatabaseSettings
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
