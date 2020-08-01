using BandwidthMonitorService.Exceptions;
using Xunit;

namespace BandwidthMonitorService.UnitTests.Exceptions
{
    public class DownloadFailedExceptionTests
    {
        [Fact]
        public void GivenUrl_WhenCreateInstance_ThenResultMessageContainsUrl_AndResultUrlEqualUrl()
        {
            // Arrange
            var message = "http://www.pop.com";

            // Act
            var sut = new DownloadFailedException(message);

            // Assert
            Assert.Contains(message, sut.Message);
            Assert.Equal(message, sut.Url);
        }
    }
}
