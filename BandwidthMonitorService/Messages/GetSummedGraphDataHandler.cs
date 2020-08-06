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
    public class GetSummedGraphDataHandler : IRequestHandler<GetSummedGraphDataQuery, GetSummedGraphDataResponse>
    {
        private readonly ISamplesService _samplesService;
        private readonly ITimestampService _timestampService;
        private readonly IMapper _mapper;
        private readonly ISampleGroupingService _sampleGroupingService;
        private readonly ISampleSummingService _sampleSummingService;

        public GetSummedGraphDataHandler(
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

        public async Task<GetSummedGraphDataResponse> Handle(
            GetSummedGraphDataQuery request,
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
            var summedSamples = _sampleSummingService.Sum(groupedSamples, Dto.Enums.Frequency.HourOfDay, Dto.Enums.SummingMode.Average);
            var samplesDto = _mapper.Map<List<Dto.Response.Sample>>(summedSamples);

            return new GetSummedGraphDataResponse()
            {
                SummedGraphData = new Dto.Response.SummedGraphData()
                {
                    Summary = new Dto.Response.Summary()
                    {
                        From = request.From,
                        To = request.To,
                        SampleCount = samples.Count,
                        Frequency = Dto.Enums.Frequency.HourOfDay,
                        SummingMode = Dto.Enums.SummingMode.Average
                    },
                    Samples = samplesDto
                }
            };
        }
    }
}
