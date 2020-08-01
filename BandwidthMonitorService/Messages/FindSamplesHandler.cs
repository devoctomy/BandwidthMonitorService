using AutoMapper;
using BandwidthMonitorService.DomainServices;
using BandwidthMonitorService.Services;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Messages
{
    public class FindSamplesHandler : IRequestHandler<FindSamplesQuery, FindSamplesResponse>
    {
        private readonly ISamplesService _samplesService;
        private readonly ITimestampService _timestampService;
        private readonly IMapper _mapper;

        public FindSamplesHandler(
            ISamplesService samplesService,
            ITimestampService timestampService,
            IMapper mapper)
        {
            _samplesService = samplesService;
            _timestampService = timestampService;
            _mapper = mapper;
        }

        public async Task<FindSamplesResponse> Handle(
            FindSamplesQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Yield();

            var timeStampFrom = request.From != null ? _timestampService.ToUnixTimestamp(request.From.Value) : new int?();
            var timeStampTo = request.To != null ? _timestampService.ToUnixTimestamp(request.To.Value) : new int?();

            var samples = _samplesService.Find(x =>
                x.Timestamp >= timeStampFrom &&
                x.Timestamp <= timeStampTo)
                .OrderBy(x => x.Timestamp);

            var samplesDto = _mapper.Map<List<Dto.Response.Sample>>(samples.ToList());

            return new FindSamplesResponse()
            {
                Samples = samplesDto
            };
        }
    }
}
