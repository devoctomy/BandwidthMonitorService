using BandwidthMonitorService.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var mockStatsService = new Mock<IServiceStats>();
            var downloadUrls = new DownloadUrls(new List<string>()
            {
                "http://www.london.com/files/file.bin",
                "http://www.frankfurt.com/files/file.bin",
                "http://www.ireland.com/files/file.bin",
                "http://www.paris.com/files/file.bin"
            });
            var sut = new BackgroundSamplerService(
                mockAppSettings.Object,
                mockSamplerService.Object,
                mockAsyncDelayService.Object,
                downloadUrls,
                mockStatsService.Object);

            mockSamplerService.Setup(x => x.Sample(
                It.IsAny<List<string>>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
                .Returns(new List<SamplerServiceResult>()
                {
                    new SamplerServiceResult()
                    {
                        Sample = new Dto.Response.Sample()
                        {
                            Url = downloadUrls[0]
                        },
                        IsSuccess = true
                    },
                    new SamplerServiceResult()
                    {
                        Sample = new Dto.Response.Sample()
                        {
                            Url = downloadUrls[1]
                        },
                        IsSuccess = true
                    },
                    new SamplerServiceResult()
                    {
                        Sample = new Dto.Response.Sample()
                        {
                            Url = downloadUrls[2]
                        },
                        IsSuccess = true
                    },
                    new SamplerServiceResult()
                    {
                        Sample = new Dto.Response.Sample()
                        {
                            Url = downloadUrls[3]
                        },
                        IsSuccess = true
                    }
                }.ToAsyncEnumerable());

            mockAsyncDelayService.Setup(x => x.Delay(
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<Action>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            mockStatsService.Setup(x => x.RegisterSample(
                It.IsAny<Dto.Response.Sample>()));

            // Act
            await sut.CollectAsync(
                true,
                CancellationToken.None);

            // Assert
            Assert.Equal(4, sut.GetSamples().Count);
            mockAsyncDelayService.Verify(x => x.Delay(
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<Action>(),
                It.IsAny<CancellationToken>()), Times.Once);
            mockStatsService.Verify(x => x.RegisterSample(
                It.IsAny<Dto.Response.Sample>()), Times.Exactly(4));
        }
    }
}
