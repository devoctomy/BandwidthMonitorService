using BandwidthMonitorService.Dto.Enums;
using System;

namespace BandwidthMonitorService.Services
{
    public class FrequencyRange
    {
        public Frequency Frequency { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
