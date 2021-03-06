﻿using System;

namespace BandwidthMonitorService.Services
{
    public class TimestampService : ITimestampService
    {
        public int ToUnixTimestamp(DateTime dateTime)
        {
            if(dateTime.Kind == DateTimeKind.Unspecified)
            {
                throw new ArgumentException("Kind of dateTime cannot be 'Unspecified'.", nameof(dateTime));
            }

            return (int)Math.Truncate((dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        }

        public DateTime FromUnixTimestamp(int timestamp)
        {
            return DateTime.SpecifyKind(
                new DateTime(1970, 1, 1).AddSeconds(timestamp),
                DateTimeKind.Utc);
        }
    }
}
