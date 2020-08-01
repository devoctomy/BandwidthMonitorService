using BandwidthMonitorService.Services;
using System;
using Xunit;

namespace BandwidthMonitorService.UnitTests.Services
{
    public class TimestampServiceTests
    {
        [Fact]
        public void GivenDateTime_WhenToUnixTimestamp_TheCorrectTimestampReturned()
        {
            // Arrange
            var value = new DateTime(2020, 01, 01, 10, 30, 25, DateTimeKind.Utc);
            var expected = 1577874625;
            var sut = new TimestampService();

            // Act
            var result = sut.ToUnixTimestamp(value);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GivenTimestamp_WhenFromUnixTimestamp_ThenCorrectDatetimeReturned()
        {
            // Arrange
            var value = 1577874625;
            var expected = new DateTime(2020, 01, 01, 10, 30, 25, DateTimeKind.Utc);
            var sut = new TimestampService();

            // Act
            var result = sut.FromUnixTimestamp(value);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
