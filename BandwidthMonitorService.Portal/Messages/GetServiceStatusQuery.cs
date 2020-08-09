using MediatR;

namespace BandwidthMonitorService.Portal.Messages
{
    public class GetServiceStatusQuery : IRequest<GetServiceStatusResponse>
    {
    }
}
