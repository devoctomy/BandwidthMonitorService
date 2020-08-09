using BandwidthMonitorService.Dto.Response;
using BandwidthMonitorService.Services;
using MongoDB.Driver.Core.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BandwidthMonitorService.UnitTests.Services
{
    public class ServiceStatsTests
    {
        [Fact]
        public void Given2Samples_WhenRegisterSamples_ThenSamplesRegistered_AndStatsUpdated()
        {
            // Arrange
            var sample1 = new Sample()
            {
                BytesRead = 100
            };
            var sample2 = new Sample()
            {
                BytesRead = 200
            };
            var sut = new ServiceStats();

            // Act
            sut.RegisterSample(sample1);
            sut.RegisterSample(sample2);

            // Assert
            Assert.Equal(300, sut.TotalBytesDownloaded);
            Assert.Equal(2, sut.TotalSamplesTaken);
        }

        [Fact]
        public void GivenRegisteredSamples_WhenReset_ThenStatsReset()
        {
            // Arrange
            var sample1 = new Sample()
            {
                BytesRead = 100
            };
            var sample2 = new Sample()
            {
                BytesRead = 200
            };
            var sut = new ServiceStats();
            sut.RegisterSample(sample1);
            sut.RegisterSample(sample2);
            var statsLastReset = sut.StatsLastReset;

            // Act
            sut.Reset();

            // Assert
            Assert.Equal(0, sut.TotalBytesDownloaded);
            Assert.Equal(0, sut.TotalSamplesTaken);
            Assert.True(statsLastReset < sut.StatsLastReset);
        }
    }
}
