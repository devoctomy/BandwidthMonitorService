using AutoMapper;
using BandwidthMonitorService.DomainServices;
using BandwidthMonitorService.Exceptions;
using BandwidthMonitorService.Services;
using Moq;
using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
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
            var mockPingService = new Mock<PingService>();

            var sut = new BackgroundSamplerService(
                mockAppSettings.Object,
                mockFileDownloaderService.Object,
                mockSamplesService.Object,
                mockMapper.Object,
                mockTimestampService.Object,
                mockPingService.Object);

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
        public async void GivenNoDownloadUrlsConfigured_WhenStartAsync_ThenServiceStarted_AndErrorRaised_AndServiceStops()
        {
            // Arrange
            var mockAppSettings = new Mock<IAppSettings>();
            var mockFileDownloaderService = new Mock<IFileDownloaderService>();
            var mockSamplesService = new Mock<ISamplesService>();
            var mockMapper = new Mock<IMapper>();
            var mockTimestampService = new Mock<ITimestampService>();
            var mockPingService = new Mock<PingService>();

            var sut = new BackgroundSamplerService(
                mockAppSettings.Object,
                mockFileDownloaderService.Object,
                mockSamplesService.Object,
                mockMapper.Object,
                mockTimestampService.Object,
                mockPingService.Object);

            var errorEvent = new ManualResetEvent(false);
            var error = false;
            sut.Error += delegate (object sender, BackgroundSamplerServiceErrorEventArgs args)
            {
                error = args.Exception is NoDownloadUrlsConfiguredException;
                errorEvent.Set();
            };

            // Act
            await sut.StartAsync(CancellationToken.None);
            errorEvent.WaitOne();

            // Assert
            Assert.True(error);
        }

        [Fact]
        public async void GivenSingleDownloadUrlConfigured_AndPingNotAllowed_WhenStartAsync_ThenServiceStarted_AndPingFailed_AndErrorRaised()
        {
            // Arrange
            var mockAppSettings = new Mock<IAppSettings>();
            var mockFileDownloaderService = new Mock<IFileDownloaderService>();
            var mockSamplesService = new Mock<ISamplesService>();
            var mockMapper = new Mock<IMapper>();
            var mockTimestampService = new Mock<ITimestampService>();
            var mockPingService = new Mock<IPingService>();
            var pingReply = new PingServiceReply()
            {
                Status = IPStatus.BadRoute
            };

            mockAppSettings.SetupGet(x => x.DownloadUrlLondon)
                .Returns("http://www.somesite.com/files/bigfile.bin");

            mockPingService.Setup(x => x.SendPingAsync(
                It.IsAny<string>()))
                .ReturnsAsync(pingReply);

            var sut = new BackgroundSamplerService(
                mockAppSettings.Object,
                mockFileDownloaderService.Object,
                mockSamplesService.Object,
                mockMapper.Object,
                mockTimestampService.Object,
                mockPingService.Object);

            var errorEvent = new ManualResetEvent(false);
            var error = false;
            sut.Error += delegate (object sender, BackgroundSamplerServiceErrorEventArgs args)
            {
                error = args.Exception is HostPingFailedException;
                errorEvent.Set();
            };

            // Act
            await sut.StartAsync(CancellationToken.None);
            errorEvent.WaitOne();

            // Assert
            Assert.True(error);
        }

        [Fact]
        public async void GivenSingleDownloadUrlConfigured_AndPingAllowed_AndDownloadNotFound_WhenStartAsync_ThenServiceStarted_AndPingSuccess_AndDownloadFailed_AndErrorRaised()
        {
            // Arrange
            var mockAppSettings = new Mock<IAppSettings>();
            var mockFileDownloaderService = new Mock<IFileDownloaderService>();
            var mockSamplesService = new Mock<ISamplesService>();
            var mockMapper = new Mock<IMapper>();
            var mockTimestampService = new Mock<ITimestampService>();
            var mockPingService = new Mock<IPingService>();
            var pingReply = new PingServiceReply()
            {
                Status = IPStatus.Success
            };
            var downloadResult = new DownloadResult()
            {
                HttpStatusCode = System.Net.HttpStatusCode.InternalServerError
            };

            mockAppSettings.SetupGet(x => x.DownloadUrlLondon)
                .Returns("http://www.somesite.com/files/bigfile.bin");

            mockPingService.Setup(x => x.SendPingAsync(
                It.IsAny<string>()))
                .ReturnsAsync(pingReply);

            mockFileDownloaderService.Setup(x => x.DownloadAndDiscardAsync(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(downloadResult);

            var sut = new BackgroundSamplerService(
                mockAppSettings.Object,
                mockFileDownloaderService.Object,
                mockSamplesService.Object,
                mockMapper.Object,
                mockTimestampService.Object,
                mockPingService.Object);

            var errorEvent = new ManualResetEvent(false);
            var error = false;
            sut.Error += delegate (object sender, BackgroundSamplerServiceErrorEventArgs args)
            {
                error = args.Exception is DownloadFailedException;
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
