using System;
using System.Collections.Generic;
using System.Text;

namespace BandwidthMonitorService.Dto.Response
{
    public class Summary
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Enums.Frequency Frequency { get; set; }
        public Enums.SummingMode SummingMode { get; set; }
        public int SampleCount { get; set; }
    }
}
