using BandwidthMonitorService.Exceptions;
using System;
using Xunit;

namespace BandwidthMonitorService.UnitTests.Exceptions
{
    public class BandwidthMonitorServiceExceptionTests
    {
        [Fact]
        public void GivenMessage_WhenCreateInstance_ThenResultMessageEqualMessage()
        {
            // Arrange
            var message = "hello world!";

            // Act
            var sut = new BandwidthMonitorServiceException(message);

            // Assert
            Assert.Equal(message, sut.Message);
        }

        [Fact]
        public void GivenMessage_AndInnerException_WhenCreateInstance_ThenResultMessageEqualMessage()
        {
            // Arrange
            var message = "hello world!";
            var innerException = new Exception();

            // Act
            var sut = new BandwidthMonitorServiceException(message, innerException);

            // Assert
            Assert.Equal(message, sut.Message);
            Assert.Equal(innerException, sut.InnerException);
        }
    }
}
