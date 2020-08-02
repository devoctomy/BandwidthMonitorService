using MediatR;
using System;

namespace BandwidthMonitorService.Messages
{
    public class SumSamplesQuery : IRequest<SumSamplesResponse>
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
