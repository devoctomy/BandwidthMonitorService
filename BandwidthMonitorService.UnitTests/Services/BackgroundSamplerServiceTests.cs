using AutoMapper;
using BandwidthMonitorService.Domain.Models;
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

            var error = false;
            sut.Error += delegate (object sender, BackgroundSamplerServiceErrorEventArgs args)
            {
                error = args.Exception is NoDownloadUrlsConfiguredException;
                var stopTask = sut.StopAsync(CancellationToken.None);
            };

            var stopped = new ManualResetEvent(false);
            sut.Stopped += delegate (object sender, EventArgs args)
            {
                stopped.Set();
            };

            // Act
            await sut.StartAsync(CancellationToken.None);
            stopped.WaitOne();

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

            var error = false;
            sut.Error += delegate (object sender, BackgroundSamplerServiceErrorEventArgs args)
            {
                error = args.Exception is HostPingFailedException;
                var stopTask = sut.StopAsync(CancellationToken.None);
            };

            var stopped = new ManualResetEvent(false);
            sut.Stopped += delegate (object sender, EventArgs args)
            {
                stopped.Set();
            };

            // Act
            await sut.StartAsync(CancellationToken.None);
            stopped.WaitOne();

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

            var error = false;
            sut.Error += delegate (object sender, BackgroundSamplerServiceErrorEventArgs args)
            {
                error = args.Exception is DownloadFailedException;
                var stopTask = sut.StopAsync(CancellationToken.None);
            };

            var stopped = new ManualResetEvent(false);
            sut.Stopped += delegate (object sender, EventArgs args)
            {
                stopped.Set();
            };

            // Act
            await sut.StartAsync(CancellationToken.None);
            stopped.WaitOne();

            // Assert
            Assert.True(error);

            mockPingService.Verify(x => x.SendPingAsync(
                It.IsAny<string>()), Times.Once);

            mockFileDownloaderService.Setup(x => x.DownloadAndDiscardAsync(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(downloadResult);
        }

        [Fact]
        public async void GivenSingleDownloadUrlConfigured_AndPingAllowed_WhenStartAsync_ThenServiceStarted_AndPingSuccess_AndDownloadSuccess_AndSampleStored()
        {
            // Arrange
            var mockAppSettings = new Mock<IAppSettings>();
            var mockFileDownloaderService = new Mock<IFileDownloaderService>();
            var mockSamplesService = new Mock<ISamplesService>();
            var mockMapper = new Mock<IMapper>();
            var mockTimestampService = new Mock<ITimestampService>();
            var mockPingService = new Mock<IPingService>();
            var downloadUrl = "http://www.somesite.com/files/bigfile.bin";
            var pingReply = new PingServiceReply()
            {
                Status = IPStatus.Success,
                RoundTripTime = 24
            };
            var downloadResult = new DownloadResult()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                TotalRead = 100,
                TotalReads = 10,
                Elapsed = new TimeSpan(0, 0, 10)
            };
            var sample = new Sample()
            {
                Id = Guid.NewGuid().ToString(),
                Url = downloadUrl,
                BytesRead = downloadResult.TotalRead,
                TotalReads = downloadResult.TotalReads,
                Elapsed = downloadResult.Elapsed,
                RoundTripTime = pingReply.RoundTripTime,
                Timestamp = 242424
            };
            var sampleDto = new Dto.Response.Sample()
            {
                Id = Guid.NewGuid().ToString(),
                Url = downloadUrl,
                BytesRead = downloadResult.TotalRead,
                TotalReads = downloadResult.TotalReads,
                Elapsed = downloadResult.Elapsed,
                RoundTripTime = pingReply.RoundTripTime,
                Timestamp = 242424
            };

            mockAppSettings.SetupGet(x => x.DownloadUrlLondon)
                .Returns(downloadUrl);

            mockPingService.Setup(x => x.SendPingAsync(
                It.IsAny<string>()))
                .ReturnsAsync(pingReply);

            mockFileDownloaderService.Setup(x => x.DownloadAndDiscardAsync(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(downloadResult);

            mockSamplesService.Setup(x => x.Create(
                It.IsAny<Sample>()))
                .Returns(sample);

            mockMapper.Setup(x => x.Map<Dto.Response.Sample>(
                It.IsAny<object>()))
                .Returns(sampleDto);

            var sut = new BackgroundSamplerService(
                mockAppSettings.Object,
                mockFileDownloaderService.Object,
                mockSamplesService.Object,
                mockMapper.Object,
                mockTimestampService.Object,
                mockPingService.Object);

            sut.DownloadSampled += delegate (object sender, EventArgs args)
            {
                var stopTask = sut.StopAsync(CancellationToken.None);
            };

            var stopped = new ManualResetEvent(false);
            sut.Stopped += delegate (object sender, EventArgs args)
            {
                stopped.Set();
            };

            // Act
            await sut.StartAsync(CancellationToken.None);
            stopped.WaitOne();

            // Assert
            var samples = sut.GetSamples();
            Assert.Single(samples);
            Assert.Equal(downloadUrl, samples[0].Url);
            mockSamplesService.Verify(x => x.Create(
                It.IsAny<Sample>()), Times.Once);
        }
    }
}
