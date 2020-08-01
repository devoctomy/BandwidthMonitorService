using MediatR;

namespace BandwidthMonitorService.Messages
{
    public class GetBackgroundSamplerServiceSamplesQuery : IRequest<GetBackgroundSamplerServiceSamplesResponse>
    {
    }
}
