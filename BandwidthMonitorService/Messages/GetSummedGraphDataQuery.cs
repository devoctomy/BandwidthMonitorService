using MediatR;
using System;

namespace BandwidthMonitorService.Messages
{
    public class GetSummedGraphDataQuery : IRequest<GetSummedGraphDataResponse>
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
