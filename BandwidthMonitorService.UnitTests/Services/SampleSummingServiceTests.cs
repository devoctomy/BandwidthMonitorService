using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BandwidthMonitorService.UnitTests.Services
{
    public class SampleSummingServiceTests
    {
        [Fact]
        public void GivenGroupedSamples_AndAverageSummingMode_WhenSum_ThenAveragedSamplesReturned()
        {
            // Arrange
            var groupedSamples = new List<IGrouping<int, Sample>>()
            {
                new Grouping<int, Sample>(1, new List<Sample>()
                {
                    new Sample()
                    {
                        Timestamp = 1,
                        BytesRead = 2424,
                        TotalReads = 100,
                        Elapsed = new System.TimeSpan(0, 0, 10),
                        RoundTripTime = 23
                    },
                    new Sample()
                    {
                        Timestamp = 2,
                        BytesRead = 2424,
                        TotalReads = 100,
                        Elapsed = new System.TimeSpan(0, 0, 13),
                        RoundTripTime = 22
                    },
                    new Sample()
                    {
                        Timestamp = 3,
                        BytesRead = 2424,
                        TotalReads = 100,
                        Elapsed = new System.TimeSpan(0, 0, 12),
                        RoundTripTime = 20
                    },
                    new Sample()
                    {
                        Timestamp = 4,
                        BytesRead = 2424,
                        TotalReads = 100,
                        Elapsed = new System.TimeSpan(0, 0, 16),
                        RoundTripTime = 30
                    },
                    new Sample()
                    {
                        Timestamp = 5,
                        BytesRead = 2424,
                        TotalReads = 100,
                        Elapsed = new System.TimeSpan(0, 0, 9),
                        RoundTripTime = 21
                    }
                }),
                new Grouping<int, Sample>(2, new List<Sample>()
                {
                    new Sample()
                    {
                        Timestamp = 6,
                        BytesRead = 1111,
                        TotalReads = 100,
                        Elapsed = new System.TimeSpan(0, 0, 3),
                        RoundTripTime = 5
                    },
                    new Sample()
                    {
                        Timestamp = 7,
                        BytesRead = 2222,
                        TotalReads = 90,
                        Elapsed = new System.TimeSpan(0, 0, 4),
                        RoundTripTime = 6
                    },
                    new Sample()
                    {
                        Timestamp = 8,
                        BytesRead = 3333,
                        TotalReads = 67,
                        Elapsed = new System.TimeSpan(0, 0, 5),
                        RoundTripTime = 7
                    },
                    new Sample()
                    {
                        Timestamp = 9,
                        BytesRead = 4444,
                        TotalReads = 90,
                        Elapsed = new System.TimeSpan(0, 0, 2),
                        RoundTripTime = 4
                    },
                    new Sample()
                    {
                        Timestamp = 10,
                        BytesRead = 5555,
                        TotalReads = 110,
                        Elapsed = new System.TimeSpan(0, 0, 3),
                        RoundTripTime = 1
                    }
                })
            };
            var sut = new SampleSummingService(new TimestampService());

            // Act
            var results = sut.Sum(
                groupedSamples,
                Dto.Enums.Frequency.HourOfDay,
                Dto.Enums.SummingMode.Average);

            // Assert
            Assert.Equal(2, results.Count);
            Assert.Equal(0, results[0].Timestamp);
            Assert.Equal(2424, results[0].BytesRead);
            Assert.Equal(100, results[0].TotalReads);
            Assert.Equal(12, results[0].Elapsed.TotalSeconds);
            Assert.Equal(23.2d, results[0].RoundTripTime);
            Assert.Equal(0, results[1].Timestamp);
            Assert.Equal(3333, results[1].BytesRead);
            Assert.Equal(91.4d, results[1].TotalReads);
            Assert.Equal(3.4d, results[1].Elapsed.TotalSeconds);
            Assert.Equal(4.6d, results[1].RoundTripTime);
        }
    }
}
