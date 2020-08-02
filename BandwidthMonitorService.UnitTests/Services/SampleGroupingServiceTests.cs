using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.Dto.Enums;
using BandwidthMonitorService.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BandwidthMonitorService.UnitTests.Services
{
    public class SampleGroupingServiceTests
    {
        [Fact]
        public void GivenRangeOfSamples_AndHourOfDayFrequency_WhenGroup_ThenSamplesCorrectlyGrouped()
        {
            // Arrange
            var mockFrequencyRangeChecker = new Mock<ISampleFrequencyRangeCheckerService>();
            var timestampService = new TimestampService();
            var sut = new SampleGroupingService(
                mockFrequencyRangeChecker.Object,
                timestampService);

            var samples = new List<Sample>()
            {
                new Sample()
                {
                    Timestamp = timestampService.ToUnixTimestamp(new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                },
                new Sample()
                {
                    Timestamp = timestampService.ToUnixTimestamp(new DateTime(2020, 1, 1, 0, 10, 0, DateTimeKind.Utc))
                },
                new Sample()
                {
                    Timestamp = timestampService.ToUnixTimestamp(new DateTime(2020, 1, 1, 0, 20, 0, DateTimeKind.Utc))
                },
                new Sample()
                {
                    Timestamp = timestampService.ToUnixTimestamp(new DateTime(2020, 1, 1, 0, 30, 0, DateTimeKind.Utc))
                },
                new Sample()
                {
                    Timestamp = timestampService.ToUnixTimestamp(new DateTime(2020, 1, 1, 0, 40, 0, DateTimeKind.Utc))
                },
                new Sample()
                {
                    Timestamp = timestampService.ToUnixTimestamp(new DateTime(2020, 1, 1, 0, 50, 0, DateTimeKind.Utc))
                },
                new Sample()
                {
                    Timestamp = timestampService.ToUnixTimestamp(new DateTime(2020, 1, 1, 6, 0, 0, DateTimeKind.Utc))
                },
                new Sample()
                {
                    Timestamp = timestampService.ToUnixTimestamp(new DateTime(2020, 1, 1, 6, 10, 0, DateTimeKind.Utc))
                },
                new Sample()
                {
                    Timestamp = timestampService.ToUnixTimestamp(new DateTime(2020, 1, 1, 6, 20, 0, DateTimeKind.Utc))
                },
                new Sample()
                {
                    Timestamp = timestampService.ToUnixTimestamp(new DateTime(2020, 1, 1, 6, 30, 0, DateTimeKind.Utc))
                },
                new Sample()
                {
                    Timestamp = timestampService.ToUnixTimestamp(new DateTime(2020, 1, 1, 6, 40, 0, DateTimeKind.Utc))
                },
                new Sample()
                {
                    Timestamp = timestampService.ToUnixTimestamp(new DateTime(2020, 1, 1, 6, 50, 0, DateTimeKind.Utc))
                },
            };

            mockFrequencyRangeChecker.Setup(x => x.GetRange(
                It.IsAny<List<Sample>>(),
                It.IsAny<Frequency>()))
                .Returns(new FrequencyRange()
                {
                    From = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    To = new DateTime(2020, 1, 1, 6, 50, 0, DateTimeKind.Utc),
                    Min = samples.First().Timestamp,
                    Max = samples.Last().Timestamp,
                    Frequency = Frequency.HourOfDay
                });

            // Act
            var grouped = sut.Group(samples, Frequency.HourOfDay);

            // Assert
            Assert.Equal(2, grouped.Count);
            Assert.Equal(6, grouped[0].Count());
            Assert.Equal(6, grouped[1].Count());
        }

        [Fact]
        public void GivenRangeOfSamples_AndHourOfDayFrequency_AndUnsupportedRangeOfSamples_WhenGroup_ThenArgumentExceptionThrown()
        {
            // Arrange
            var mockFrequencyRangeChecker = new Mock<ISampleFrequencyRangeCheckerService>();
            var timestampService = new TimestampService();
            var sut = new SampleGroupingService(
                mockFrequencyRangeChecker.Object,
                timestampService);

            var samples = new List<Sample>()
            {
            };

            mockFrequencyRangeChecker.Setup(x => x.GetRange(
                It.IsAny<List<Sample>>(),
                It.IsAny<Frequency>()))
                .Returns(new FrequencyRange()
                {
                    From = new DateTime(2020, 1, 1),
                    To = new DateTime(2020, 1, 2)
                });

            // Act / Assert
            Assert.Throws<ArgumentException>(() =>
            {
                sut.Group(samples, Frequency.HourOfDay);
            });
        }
    }
}
