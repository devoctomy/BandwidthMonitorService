using Microsoft.AspNetCore.SignalR;
using System;
using System.Net;

namespace BandwidthMonitorService.Services
{
    public class DownloadResult
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public long TotalRead { get; set; }
        public long TotalReads { get; set; }
        public TimeSpan Elapsed { get; set; }
    }
}
