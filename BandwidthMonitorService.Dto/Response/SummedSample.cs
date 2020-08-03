using BandwidthMonitorService.Dto.Enums;
using System;

namespace BandwidthMonitorService.Dto.Response
{
    public class SummedSample : Sample
    {
        public Frequency Frequency { get; set; }
        public int SampleCount { get; set; }
        public int? FrequencyIndex { get; set; }
    }
}
