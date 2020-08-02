using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.Dto.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BandwidthMonitorService.Services
{
    public class SampleSummingService : ISampleSummingService
    {
        public List<Sample> Sum(
            IEnumerable<IGrouping<int, Sample>> groupedSamples,
            SummingMode summingMode)
        {
            if(summingMode != SummingMode.Average)
            {
                throw new NotImplementedException($"Summing mode '{summingMode}' not currently implemented.");
            }

            var summedSamples = new List<Sample>();
            foreach (var curGroup in groupedSamples)
            {
                summedSamples.Add(new Sample()
                {
                    Id = Guid.NewGuid().ToString(),
                    Timestamp = curGroup.First().Timestamp,
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
