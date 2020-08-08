using System;
using System.Collections.Generic;
using System.Text;

namespace BandwidthMonitorService.Client.Services
{
    public class BandwidthMonitorServiceClientAttribute : Attribute
    {
        public string UniqueName { get; set; }
    }
}
