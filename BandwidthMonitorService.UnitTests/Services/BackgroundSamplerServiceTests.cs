using AutoMapper;
using BandwidthMonitorService.DomainServices;
using BandwidthMonitorService.Services;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace BandwidthMonitorService.UnitTests.Services
{
    public class BackgroundSamplerServiceTests
    {
        [Fact]
        public async void GivenInstanceOfService_WhenStartAsync_ThenServiceStarted()
        {
            // Arrange
            var mockAppSettings = new Mock<IAppSettings>();
            var mockFileDownloaderService = new Mock<IFileDownloaderService>();
            var mockSamplesService = new Mock<ISamplesService>();
            var mockMapper = new Mock<IMapper>();
            var mockTimestampService = new Mock<ITimestampService>();

            var sut = new BackgroundSamplerService(
                mockAppSettings.Object,
                mockFileDownloaderService.Object,
                mockSamplesService.Object,
                mockMapper.Object,
                mockTimestampService.Object);

            var startedEvent = new ManualResetEvent(false);
            var started = false;
            sut.Started += delegate (object sender, EventArgs args)
            {
                started = true;
                ((BackgroundSamplerService)sender).StopAsync(CancellationToken.None).GetAwaiter().GetResult();
                startedEvent.Set();
            };

            // Act
            await sut.StartAsync(CancellationToken.None);
            startedEvent.WaitOne();

            // Assert
            Assert.True(started);
        }

        [Fact]
        public async void GivenNoDownloadUrlsConfigured_WhenStartAsync_ThenServiceStarted_AndServiceRaisesError_AndServiceStops()
        {
            // Arrange
            var mockAppSettings = new Mock<IAppSettings>();
            var mockFileDownloaderService = new Mock<IFileDownloaderService>();
            var mockSamplesService = new Mock<ISamplesService>();
            var mockMapper = new Mock<IMapper>();
            var mockTimestampService = new Mock<ITimestampService>();

            var sut = new BackgroundSamplerService(
                mockAppSettings.Object,
                mockFileDownloaderService.Object,
                mockSamplesService.Object,
                mockMapper.Object,
                mockTimestampService.Object);

            var errorEvent = new ManualResetEvent(false);
            var error = false;
            sut.Error += delegate (object sender, EventArgs args)
            {
                error = true;
                errorEvent.Set();
            };

            // Act
            await sut.StartAsync(CancellationToken.None);
            errorEvent.WaitOne();

            // Assert
            Assert.True(error);
        }
    }
}
