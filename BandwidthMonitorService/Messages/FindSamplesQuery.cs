using MediatR;
using System;

namespace BandwidthMonitorService.Messages
{
    public class FindSamplesQuery : IRequest<FindSamplesResponse>
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
