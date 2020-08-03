using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.Dto.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BandwidthMonitorService.Services
{
    public class SampleSummingService : ISampleSummingService
    {
        private readonly ITimestampService _timestampService;

        public SampleSummingService(ITimestampService timestampService)
        {
            _timestampService = timestampService;
        }

        public List<Dto.Response.SummedSample> Sum(
            List<IGrouping<int, Sample>> groupedSamples,
            Frequency freuency,
            SummingMode summingMode)
        {
            if(summingMode != SummingMode.Average)
            {
                throw new NotImplementedException($"Summing mode '{summingMode}' not currently implemented.");
            }

            var summedSamples = new List<Dto.Response.SummedSample>();
            foreach (var curGroup in groupedSamples)
            {
                var firstSampleOnTheHourTimestamp = _timestampService.FromUnixTimestamp(curGroup.First().Timestamp);
                firstSampleOnTheHourTimestamp = new DateTime(
                    firstSampleOnTheHourTimestamp.Year,
                    firstSampleOnTheHourTimestamp.Month,
                    firstSampleOnTheHourTimestamp.Day,
                    firstSampleOnTheHourTimestamp.Hour,
                    0,
                    0,
                    DateTimeKind.Utc);
                var timestamp = _timestampService.ToUnixTimestamp(firstSampleOnTheHourTimestamp);
                summedSamples.Add(new Dto.Response.SummedSample()
                {
                    Id = Guid.NewGuid().ToString(),
                    Timestamp = timestamp,
                    Url = "Averaged Sample",
                    BytesRead = curGroup.Average(x => x.BytesRead),
                    TotalReads = curGroup.Average(x => x.TotalReads),
                    Elapsed = TimeSpan.FromMilliseconds(curGroup.Average(x => x.Elapsed.TotalMilliseconds)),
                    RoundTripTime = curGroup.Average(x => x.RoundTripTime),
                    SampleCount = curGroup.Count(),
                    Frequency = freuency,
                    FrequencyIndex = GetFrequencyIndex(freuency, timestamp)
                });
            }

            return summedSamples.OrderBy(x => x.FrequencyIndex).ToList();
        }

        private int? GetFrequencyIndex(
            Frequency frequency,
            int timestamp)
        {
            switch(frequency)
            {
                case Frequency.HourOfDay:
                    {
                        return _timestampService.FromUnixTimestamp(timestamp).Hour;
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
