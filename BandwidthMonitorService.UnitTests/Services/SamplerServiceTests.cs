using AutoMapper;
using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.DomainServices;
using BandwidthMonitorService.Exceptions;
using BandwidthMonitorService.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Xunit;

namespace BandwidthMonitorService.UnitTests.Services
{
    public class SamplerServiceTests
    {
        [Fact]
        public async void GivenUrl_AndHostNotPingable_WhenSample_ThenErrorReturned()
        {
            // Arrange
            var mockAppSettings = new Mock<IAppSettings>();
            var mockFileDownloaderService = new Mock<IFileDownloaderService>();
            var mockSamplesService = new Mock<ISamplesService>();
            var mockMapper = new Mock<IMapper>();
            var mockTimestampService = new Mock<ITimestampService>();
            var mockPingService = new Mock<IPingService>();

            var sut = new SamplerService(
                mockAppSettings.Object,
                mockFileDownloaderService.Object,
                mockSamplesService.Object,
                mockMapper.Object,
                mockTimestampService.Object,
                mockPingService.Object);

            var url = "http://www.pop.com/file.bin";

            mockPingService.Setup(x => x.SendPingAsync(
                It.IsAny<string>()))
                .ReturnsAsync(new PingServiceReply()
                {
                    Status = System.Net.NetworkInformation.IPStatus.BadRoute
                });

            // Act
            var result = await sut.Sample(
                url,
                CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.IsType<HostPingFailedException>(result.Exception);

            mockPingService.Verify(x => x.SendPingAsync(
                It.Is<string>(y => y == new Uri(url).Host)), Times.Once);
        }

        [Fact]
        public async void GivenUrl_AndHostPingable_AndFileNotFound_WhenSample_ThenErrorReturned()
        {
            // Arrange
            var mockAppSettings = new Mock<IAppSettings>();
            var mockFileDownloaderService = new Mock<IFileDownloaderService>();
            var mockSamplesService = new Mock<ISamplesService>();
            var mockMapper = new Mock<IMapper>();
            var mockTimestampService = new Mock<ITimestampService>();
            var mockPingService = new Mock<IPingService>();

            var sut = new SamplerService(
                mockAppSettings.Object,
                mockFileDownloaderService.Object,
                mockSamplesService.Object,
                mockMapper.Object,
                mockTimestampService.Object,
                mockPingService.Object);

            var url = "http://www.pop.com/file.bin";

            mockPingService.Setup(x => x.SendPingAsync(
                It.IsAny<string>()))
                .ReturnsAsync(new PingServiceReply()
                {
                    Status = System.Net.NetworkInformation.IPStatus.Success
                });

            mockFileDownloaderService.Setup(x => x.DownloadAndDiscardAsync(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DownloadResult()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound
                });

            // Act
            var result = await sut.Sample(
                url,
                CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.IsType<DownloadFailedException>(result.Exception);

            mockPingService.Verify(x => x.SendPingAsync(
                It.Is<string>(y => y == new Uri(url).Host)), Times.Once);

            mockFileDownloaderService.Verify(x => x.DownloadAndDiscardAsync(
                It.Is<string>(y => y == url),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void GivenUrl_AndHostPingable_AndFileFound_WhenSample_ThenSampleStored_AndSuccessReturned()
        {
            // Arrange
            var mockAppSettings = new Mock<IAppSettings>();
            var mockFileDownloaderService = new Mock<IFileDownloaderService>();
            var mockSamplesService = new Mock<ISamplesService>();
            var mockMapper = new Mock<IMapper>();
            var mockTimestampService = new Mock<ITimestampService>();
            var mockPingService = new Mock<IPingService>();

            var sut = new SamplerService(
                mockAppSettings.Object,
                mockFileDownloaderService.Object,
                mockSamplesService.Object,
                mockMapper.Object,
                mockTimestampService.Object,
                mockPingService.Object);

            var url = "http://www.pop.com/file.bin";

            mockPingService.Setup(x => x.SendPingAsync(
                It.IsAny<string>()))
                .ReturnsAsync(new PingServiceReply()
                {
                    Status = System.Net.NetworkInformation.IPStatus.Success
                });

            mockFileDownloaderService.Setup(x => x.DownloadAndDiscardAsync(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DownloadResult()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK
                });

            mockSamplesService.Setup(x => x.Create(
                It.IsAny<Sample>()))
                .Returns(new Sample());

            // Act
            var result = await sut.Sample(
                url,
                CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Exception);

            mockPingService.Verify(x => x.SendPingAsync(
                It.Is<string>(y => y == new Uri(url).Host)), Times.Once);

            mockFileDownloaderService.Verify(x => x.DownloadAndDiscardAsync(
                It.Is<string>(y => y == url),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()), Times.Once);

            mockSamplesService.Verify(x => x.Create(
                It.Is<Sample>(y => y.Url == url)), Times.Once);
        }

        [Fact]
        public async void GivenListOfUrls_WhenSample_ThenEachUrlSampledAndResultsReturned()
        {
            // Arrange
            var mockAppSettings = new Mock<IAppSettings>();
            var mockFileDownloaderService = new Mock<IFileDownloaderService>();
            var mockSamplesService = new Mock<ISamplesService>();
            var mockMapper = new Mock<IMapper>();
            var mockTimestampService = new Mock<ITimestampService>();
            var mockPingService = new Mock<IPingService>();

            var sut = new SamplerService(
                mockAppSettings.Object,
                mockFileDownloaderService.Object,
                mockSamplesService.Object,
                mockMapper.Object,
                mockTimestampService.Object,
                mockPingService.Object);

            var urls = new List<string>()
            {
                "http://www.pop.com/file1.bin",
                "http://www.pop.com/file2.bin",
                "http://www.pop.com/file3.bin",
                "http://www.pop.com/file4.bin"
            };

            mockPingService.Setup(x => x.SendPingAsync(
                It.IsAny<string>()))
                .ReturnsAsync(new PingServiceReply()
                {
                    Status = System.Net.NetworkInformation.IPStatus.Success
                });

            mockFileDownloaderService.Setup(x => x.DownloadAndDiscardAsync(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DownloadResult()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK
                });

            mockSamplesService.Setup(x => x.Create(
                It.IsAny<Sample>()))
                .Returns(new Sample());

            // Act
            var results = await sut.Sample(
                urls,
                CancellationToken.None);

            // Assert
            Assert.Equal(4, results.Count);

            mockPingService.Verify(x => x.SendPingAsync(
                It.IsAny<string>()), Times.Exactly(4));

            mockFileDownloaderService.Verify(x => x.DownloadAndDiscardAsync(
                It.Is<string>(y => urls.Contains(y)),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()), Times.Exactly(4));

            mockSamplesService.Verify(x => x.Create(
                It.Is<Sample>(y => urls.Contains(y.Url))), Times.Exactly(4));
        }
    }
}
