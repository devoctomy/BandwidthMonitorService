using MediatR;
using System;

namespace BandwidthMonitorService.Messages
{
    public class GetServiceStatusQuery : IRequest<GetServiceStatusResponse>
    {
    }
}
