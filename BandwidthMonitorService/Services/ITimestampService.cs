using System;

namespace BandwidthMonitorService.Services
{
    public interface ITimestampService
    {
        int ToUnixTimestamp(DateTime dateTime);
        DateTime FromUnixTimestamp(int timestamp);
    }
}
