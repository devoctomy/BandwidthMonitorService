using BandwidthMonitorService.Services;
using Moq;
using Moq.Protected;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BandwidthMonitorService.UnitTests.Services
{
    public class FileDownloaderServiceTests
    {
        [Fact]
        public async void GivenValidUrl_AndContentSize16Mb_AndBufferSize1Mb_WhenDownloadAndDiscard_ThenFileDownloaded_AndCorrectResultsReturned()
        {
            // Arrange
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var url = "http://www.somesite.com/files/bigfile.bin";
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var testClient = new HttpClient(mockHttpMessageHandler.Object);

            mockHttpClientFactory.Setup(x => x.CreateClient(
                It.IsAny<string>()))
                .Returns(testClient);

            var sut = new FileDownloaderService(mockHttpClientFactory.Object);
            var contentSize = 16777216;
            var bufferSize = 1048576;

            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StreamContent(CreateMemoryStream(contentSize))
                });

            // Act
            var result = await sut.DownloadAndDiscardAsync(url, bufferSize, CancellationToken.None);

            // Assert
            Assert.True(result.Elapsed > TimeSpan.Zero);
            Assert.Equal(contentSize, result.TotalRead);
            Assert.Equal(16, result.TotalReads);
        }

        [Fact]
        public async void GivenInvalidUrl_WhenDownloadAndDiscard_ThenFileDownloadFailed_AndCorrectResultReturned()
        {
            // Arrange
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var url = "http://www.somesite.com/files/bigfile.bin";
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var testClient = new HttpClient(mockHttpMessageHandler.Object);

            mockHttpClientFactory.Setup(x => x.CreateClient(
                It.IsAny<string>()))
                .Returns(testClient);

            var sut = new FileDownloaderService(mockHttpClientFactory.Object);
            var bufferSize = 1048576;

            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Content = null
                });

            // Act
            var result = await sut.DownloadAndDiscardAsync(url, bufferSize, CancellationToken.None);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.HttpStatusCode);
        }

        private MemoryStream CreateMemoryStream(int size)
        {
            var stream = new MemoryStream(new byte[size]);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
