using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace BandwidthMonitorService.UnitTests.Services
{
    public class SampleFrequencyRangeCheckerServiceTests
    {
        [Fact]
        public void GivenRangeOfSamples_AndHourOfDayFrequency_WhenGetRange_ThenCorrectRangeValuesReturned()
        {
            // Arrange
            var timeStampService = new TimestampService();
            var sut = new SampleFrequencyRangeCheckerService(timeStampService);

            var timeStamps = new List<DateTime>()
            {
                new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 1, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 2, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 3, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 4, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 5, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 6, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 7, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 8, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 9, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 11, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 13, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 14, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 15, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 16, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 17, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 18, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 19, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 20, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 21, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 22, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 1, 23, 0, 0, DateTimeKind.Utc),
            };
            var testSamples = timeStamps.Select(x => new Sample()
            {
                Timestamp = timeStampService.ToUnixTimestamp(x)
            });

            // Act
            var range = sut.GetRange(testSamples, Dto.Enums.Frequency.HourOfDay);

            // Assert
            Assert.Equal(Dto.Enums.Frequency.HourOfDay, range.Frequency);
            Assert.Equal(0, range.Min);
            Assert.Equal(23, range.Max);
        }

        [Fact]
        public void GivenRangeOfSamples_AndDayOfMonthFrequency_WhenGetRange_ThenCorrectRangeValuesReturned()
        {
            // Arrange
            var timeStampService = new TimestampService();
            var sut = new SampleFrequencyRangeCheckerService(timeStampService);

            var timeStamps = new List<DateTime>()
            {
                new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 2, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 3, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 4, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 5, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 6, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 7, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 8, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 9, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 10, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 11, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 12, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 13, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 14, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 15, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 16, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 17, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 18, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 19, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 21, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 22, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 23, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 24, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 25, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 26, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 27, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 28, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 29, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 30, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
            };
            var testSamples = timeStamps.Select(x => new Sample()
            {
                Timestamp = timeStampService.ToUnixTimestamp(x)
            });

            // Act
            var range = sut.GetRange(testSamples, Dto.Enums.Frequency.DayOfMonth);

            // Assert
            Assert.Equal(Dto.Enums.Frequency.DayOfMonth, range.Frequency);
            Assert.Equal(1, range.Min);
            Assert.Equal(31, range.Max);
        }

        [Fact]
        public void GivenRangeOfSamples_AndWeekOfYearFrequency_WhenGetRange_ThenCorrectRangeValuesReturned()
        {
            // Arrange
            var timeStampService = new TimestampService();
            var sut = new SampleFrequencyRangeCheckerService(timeStampService);

            var timeStamps = new List<DateTime>()
            {
                new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 6, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 13, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 1, 27, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 2, 3, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 2, 10, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 2, 17, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 2, 24, 0, 0, 0, DateTimeKind.Utc),
            };
            var testSamples = timeStamps.Select(x => new Sample()
            {
                Timestamp = timeStampService.ToUnixTimestamp(x)
            });

            // Act
            var range = sut.GetRange(testSamples, Dto.Enums.Frequency.WeekOfYear);

            // Assert
            Assert.Equal(Dto.Enums.Frequency.WeekOfYear, range.Frequency);
            Assert.Equal(1, range.Min);
            Assert.Equal(9, range.Max);
        }

        [Fact]
        public void GivenRangeOfSamples_AndMonthOfYearFrequency_WhenGetRange_ThenCorrectRangeValuesReturned()
        {
            // Arrange
            var timeStampService = new TimestampService();
            var sut = new SampleFrequencyRangeCheckerService(timeStampService);

            var timeStamps = new List<DateTime>()
            {
                new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 9, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 11, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2020, 12, 1, 0, 0, 0, DateTimeKind.Utc)
            };
            var testSamples = timeStamps.Select(x => new Sample()
            {
                Timestamp = timeStampService.ToUnixTimestamp(x)
            });

            // Act
            var range = sut.GetRange(testSamples, Dto.Enums.Frequency.MonthOfYear);

            // Assert
            Assert.Equal(Dto.Enums.Frequency.MonthOfYear, range.Frequency);
            Assert.Equal(1, range.Min);
            Assert.Equal(12, range.Max);
        }

        [Fact]
        public void GivenRangeOfSamples_AndYearFrequency_WhenGetRange_ThenCorrectRangeValuesReturned()
        {
            // Arrange
            var timeStampService = new TimestampService();
            var sut = new SampleFrequencyRangeCheckerService(timeStampService);

            var timeStamps = new List<DateTime>()
            {
                new DateTime(1982, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(1983, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(1984, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(1985, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(1986, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(1987, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(1988, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(1989, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };
            var testSamples = timeStamps.Select(x => new Sample()
            {
                Timestamp = timeStampService.ToUnixTimestamp(x)
            });

            // Act
            var range = sut.GetRange(testSamples, Dto.Enums.Frequency.Year);

            // Assert
            Assert.Equal(Dto.Enums.Frequency.Year, range.Frequency);
            Assert.Equal(1982, range.Min);
            Assert.Equal(1990, range.Max);
        }
    }
}
