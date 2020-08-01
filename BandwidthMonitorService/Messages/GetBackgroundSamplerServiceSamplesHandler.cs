using BandwidthMonitorService.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Messages
{
    public class GetBackgroundSamplerServiceSamplesHandler : IRequestHandler<GetBackgroundSamplerServiceSamplesQuery, GetBackgroundSamplerServiceSamplesResponse>
    {
        private readonly IBackgroundSamplerService _backgroundSamplerService;

        public GetBackgroundSamplerServiceSamplesHandler(IBackgroundSamplerService backgroundSamplerService)
        {
            _backgroundSamplerService = backgroundSamplerService;
        }

        public async Task<GetBackgroundSamplerServiceSamplesResponse> Handle(
            GetBackgroundSamplerServiceSamplesQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Yield();

            var response = new GetBackgroundSamplerServiceSamplesResponse()
            {
                Samples = _backgroundSamplerService.GetSamples()
            };

            return response;
        }
    }
}
