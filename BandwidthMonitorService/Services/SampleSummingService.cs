using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.Dto.Enums;
using Microsoft.AspNetCore.DataProtection;
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

        public List<Sample> Sum(
            List<IGrouping<int, Sample>> groupedSamples,
            SummingMode summingMode)
        {
            if(summingMode != SummingMode.Average)
            {
                throw new NotImplementedException($"Summing mode '{summingMode}' not currently implemented.");
            }

            var summedSamples = new List<Sample>();
            foreach (var curGroup in groupedSamples)
            {
                var firstSampleOnTheHourTimestamp = _timestampService.FromUnixTimestamp(curGroup.First().Timestamp);
                firstSampleOnTheHourTimestamp = new DateTime(
                    firstSampleOnTheHourTimestamp.Year,
                    firstSampleOnTheHourTimestamp.Month,
                    firstSampleOnTheHourTimestamp.Day,
                    firstSampleOnTheHourTimestamp.Hour,
                    0,
                    0);
                summedSamples.Add(new Sample()
                {
                    Id = Guid.NewGuid().ToString(),
                    Timestamp = _timestampService.ToUnixTimestamp(firstSampleOnTheHourTimestamp),
                    Url = "Averaged Sample",
                    BytesRead = curGroup.Average(x => x.BytesRead),
                    TotalReads = curGroup.Average(x => x.TotalReads),
                    Elapsed = TimeSpan.FromMilliseconds(curGroup.Average(x => x.Elapsed.TotalMilliseconds)),
                    RoundTripTime = curGroup.Average(x => x.RoundTripTime)
                });
            }

            return summedSamples;
        }
    }
}
