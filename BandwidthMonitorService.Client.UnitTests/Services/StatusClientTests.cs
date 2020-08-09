using BandwidthMonitorService.Client.Services;
using BandwidthMonitorService.Dto.Response;
using Moq;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using Xunit;

namespace BandwidthMonitorService.Client.UnitTests.Services
{
    public class StatusClientTests
    {
        [Fact]
        public async void GivenNoParams_AndServiceOk_WhenGetServiceStatus_ThenRequestMade_AndResponseReturned()
        {
            // Arrange
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockTestableHttpeMessageHandler = new Mock<TestableHttpMessageHandler>()
            {
                CallBase = true
            };
            var responseValue = new ServiceStatus()
            {
                Uptime = new TimeSpan(1, 1, 1)
            };

            mockTestableHttpeMessageHandler.Setup(x => x.Send(
                It.IsAny<HttpRequestMessage>()))
                .Returns(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(responseValue))
                });

            mockHttpClientFactory.Setup(x => x.CreateClient(
                It.IsAny<string>()))
                .Returns(new HttpClient(mockTestableHttpeMessageHandler.Object)
                {
                    BaseAddress = new Uri("http://localhost:5000")
                });

            var sut = new StatusClient(mockHttpClientFactory.Object);

            // Act
            var response = await sut.GetServiceStatusAsync(CancellationToken.None);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.HttpStatusCode);
            Assert.Equal(JsonConvert.SerializeObject(responseValue), JsonConvert.SerializeObject(response.Value));
        }

        [Fact]
        public async void GivenNoParams_AndServiceFaulty_WhenGetServiceStatus_ThenRequestMade_AndErrorResponseReturned()
        {
            // Arrange
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockTestableHttpeMessageHandler = new Mock<TestableHttpMessageHandler>()
            {
                CallBase = true
            };

            mockTestableHttpeMessageHandler.Setup(x => x.Send(
                It.IsAny<HttpRequestMessage>()))
                .Returns(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                });

            mockHttpClientFactory.Setup(x => x.CreateClient(
                It.IsAny<string>()))
                .Returns(new HttpClient(mockTestableHttpeMessageHandler.Object)
                {
                    BaseAddress = new Uri("http://localhost:5000")
                });

            var sut = new StatusClient(mockHttpClientFactory.Object);

            // Act
            var response = await sut.GetServiceStatusAsync(CancellationToken.None);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.HttpStatusCode);
            Assert.Null(response.Value);
        }
    }
}
