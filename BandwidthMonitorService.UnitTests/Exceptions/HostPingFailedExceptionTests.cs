using BandwidthMonitorService.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using Xunit;

namespace BandwidthMonitorService.UnitTests.Exceptions
{
    public class HostPingFailedExceptionTests
    {
        [Fact]
        public void GivenHost_AndStatus_WhenCreateInstance_ThenResultMessageContainsUrl_AndResultUrlEqualUrl()
        {
            // Arrange
            var host = "www.pop.com";
            var status = IPStatus.BadOption;

            // Act
            var sut = new HostPingFailedException(host, status);

            // Assert
            Assert.Contains(host, sut.Message);
            Assert.Contains(status.ToString(), sut.Message);
            Assert.Equal(host, sut.Host);
            Assert.Equal(status, sut.Status);
        }
    }
}
