using BandwidthMonitorService.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BandwidthMonitorService.UnitTests.Services
{
    public class BackgroundSamplerServiceTests
    {
        [Fact]
        public async void GivenAllValidDownloadUrls_WhenCollect_ThenSamplesCollected__AndAddedToLatestSamples_AndDelaysProcessed()
        {
            // Arrange
            var mockAppSettings = new Mock<IAppSettings>();
            var mockSamplerService = new Mock<ISamplerService>();
            var mockAsyncDelayService = new Mock<IAsyncDelayService>();
            var sut = new BackgroundSamplerService(
                mockAppSettings.Object,
                mockSamplerService.Object,
                mockAsyncDelayService.Object);

            mockAppSettings.SetupGet(x => x.DownloadUrlLondon).Returns("http://www.london.com/files/file.bin");
            mockAppSettings.SetupGet(x => x.DownloadUrlFrankfurt).Returns("http://www.frankfurt.com/files/file.bin");
            mockAppSettings.SetupGet(x => x.DownloadUrlIreland).Returns("http://www.ireland.com/files/file.bin");
            mockAppSettings.SetupGet(x => x.DownloadUrlParis).Returns("http://www.paris.com/files/file.bin");

            mockSamplerService.Setup(x => x.Sample(
                It.IsAny<List<string>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SamplerServiceResult>()
                {
                    new SamplerServiceResult()
                    {
                        Sample = new Dto.Response.Sample()
                        {
                            Url = mockAppSettings.Object.DownloadUrlLondon
                        },
                        IsSuccess = true
                    },
                    new SamplerServiceResult()
                    {
                        Sample = new Dto.Response.Sample()
                        {
                            Url = mockAppSettings.Object.DownloadUrlFrankfurt
                        },
                        IsSuccess = true
                    },
                    new SamplerServiceResult()
                    {
                        Sample = new Dto.Response.Sample()
                        {
                            Url = mockAppSettings.Object.DownloadUrlIreland
                        },
                        IsSuccess = true
                    },
                    new SamplerServiceResult()
                    {
                        Sample = new Dto.Response.Sample()
                        {
                            Url = mockAppSettings.Object.DownloadUrlParis
                        },
                        IsSuccess = true
                    }
                });

            mockAsyncDelayService.Setup(x => x.Delay(
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<Action>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await sut.CollectAsync(CancellationToken.None);

            // Assert
            Assert.Equal(4, sut.GetSamples().Count);
            mockAsyncDelayService.Verify(x => x.Delay(
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<Action>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
