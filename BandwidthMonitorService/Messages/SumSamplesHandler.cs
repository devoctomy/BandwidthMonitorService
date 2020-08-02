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
    public class SumSamplesHandler : IRequestHandler<SumSamplesQuery, SumSamplesResponse>
    {
        private readonly ISamplesService _samplesService;
        private readonly ITimestampService _timestampService;
        private readonly IMapper _mapper;
        private readonly ISampleGroupingService _sampleGroupingService;
        private readonly ISampleSummingService _sampleSummingService;

        public SumSamplesHandler(
            ISamplesService samplesService,
            ITimestampService timestampService,
            IMapper mapper,
            ISampleGroupingService sampleGroupingService,
            ISampleSummingService sampleSummingService)
        {
            _samplesService = samplesService;
            _timestampService = timestampService;
            _mapper = mapper;
            _sampleGroupingService = sampleGroupingService;
            _sampleSummingService = sampleSummingService;
        }

        public async Task<SumSamplesResponse> Handle(
            SumSamplesQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Yield();

            var timeStampFrom = request.From != null ? _timestampService.ToUnixTimestamp(request.From.Value) : new int?();
            var timeStampTo = request.To != null ? _timestampService.ToUnixTimestamp(request.To.Value) : new int?();

            var samples = _samplesService.Find(x =>
                x.Timestamp >= timeStampFrom &&
                x.Timestamp <= timeStampTo)
                .OrderBy(x => x.Timestamp)
                .ToList();

            var groupedSamples = _sampleGroupingService.Group(samples, Dto.Enums.Frequency.HourOfDay);
            var summedSamples = _sampleSummingService.Sum(groupedSamples, Dto.Enums.SummingMode.Average);
            var samplesDto = _mapper.Map<List<Dto.Response.Sample>>(summedSamples);

            return new SumSamplesResponse()
            {
                Samples = samplesDto
            };
        }
    }
}
