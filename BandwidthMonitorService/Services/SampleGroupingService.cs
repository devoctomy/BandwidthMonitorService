using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.Dto.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BandwidthMonitorService.Services
{
    public class SampleGroupingService : ISampleGroupingService
    {
        private readonly ISampleFrequencyRangeCheckerService _sampleFrequencyRangeCheckerService;
        private readonly ITimestampService _timestampService;
        private readonly ISampleSummingService _sampleSummingService;

        public SampleGroupingService(
            ISampleFrequencyRangeCheckerService sampleFrequencyRangeCheckerService,
            ITimestampService timestampService,
            ISampleSummingService sampleSummingService)
        {
            _sampleFrequencyRangeCheckerService = sampleFrequencyRangeCheckerService;
            _timestampService = timestampService;
            _sampleSummingService = sampleSummingService;
        }

        public List<Sample> Group(
            List<Sample> orderedSamples,
            Frequency frequency)
        {
            var range = _sampleFrequencyRangeCheckerService.GetRange(
                orderedSamples,
                frequency);

            var grouped = default(IEnumerable<IGrouping<int, Sample>>);
            switch(frequency)
            {
                case Frequency.HourOfDay:
                    {
                        if(range.From.DayOfYear != range.To.DayOfYear)
                        {
                            throw new ArgumentException("Samples must be all in the same day when grouping by HourOfDay");
                        }

                        grouped = orderedSamples.GroupBy(x => _timestampService.FromUnixTimestamp(x.Timestamp).Hour);
                        break;
                    }
            }

            var summed = _sampleSummingService.Sum(
                grouped,
                SummingMode.Average);

            return summed;
        }
    }
}
