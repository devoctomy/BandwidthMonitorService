using BandwidthMonitorService.Client.Services;
using BandwidthMonitorService.Dto.Request;
using BandwidthMonitorService.Dto.Response;
using Moq;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using Xunit;

namespace BandwidthMonitorService.Client.UnitTests.Services
{
    public class ReportDataClientTests
    {
        [Fact]
        public async void GivenGetSummedGraphDataQuery_AndServiceOk_WhenGetSummedGraphData_ThenRequestMade_AndResponseReturned()
        {
            // Arrange
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockTestableHttpeMessageHandler = new Mock<TestableHttpMessageHandler>()
            { 
                CallBase = true
            };
            var responseValue = new SummedGraphData()
            {
                Summary = new Summary()
                {
                    From = new DateTime(2020, 1, 1, 0, 0, 0),
                    To = new DateTime(2020, 1, 31, 23, 59, 59),
                },
                Samples = new System.Collections.Generic.List<Sample>()
                {
                    new Sample()
                    {
                        Id = Guid.NewGuid().ToString()
                    }
                }
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

            var query = new GetSummedGraphDataQuery()
            {
                From = new DateTime(2020, 1, 1, 0, 0, 0),
                To = new DateTime(2020, 1, 31, 23, 59, 59)
            };
            var sut = new ReportDataClient(mockHttpClientFactory.Object);

            // Act
            var response = await sut.GetSummedGraphDataAsync(query);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.HttpStatusCode);
            Assert.Equal(JsonConvert.SerializeObject(responseValue), JsonConvert.SerializeObject(response.Value));
        }

        [Fact]
        public async void GivenGetSummedGraphDataQuery_AndServiceFaulty_WhenGetSummedGraphData_ThenRequestMade_AndErrorResponseReturned()
        {
            // Arrange
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockTestableHttpeMessageHandler = new Mock<TestableHttpMessageHandler>()
            {
                CallBase = true
            };
            var responseValue = new SummedGraphData()
            {
                Summary = new Summary()
                {
                    From = new DateTime(2020, 1, 1, 0, 0, 0),
                    To = new DateTime(2020, 1, 31, 23, 59, 59),
                },
                Samples = new System.Collections.Generic.List<Sample>()
                {
                    new Sample()
                    {
                        Id = Guid.NewGuid().ToString()
                    }
                }
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

            var query = new GetSummedGraphDataQuery()
            {
                From = new DateTime(2020, 1, 1, 0, 0, 0),
                To = new DateTime(2020, 1, 31, 23, 59, 59)
            };
            var sut = new ReportDataClient(mockHttpClientFactory.Object);

            // Act
            var response = await sut.GetSummedGraphDataAsync(query);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.HttpStatusCode);
            Assert.Null(response.Value);
        }
    }
}
