using BandwidthMonitorService.Messages;
using BandwidthMonitorService.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using BandwidthMonitorService.Dto.Response;

namespace BandwidthMonitorService.UnitTests.Controllers
{
    public class ReportDataControllerTests
    {
        [Fact]
        public async void GivenNoParams_WhenGetLatestRawSamples_ThenRequestSent_AndResponseReturned()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ReportDataController>>();
            var mockMediator = new Mock<IMediator>();
            var sut = new ReportDataController(
                mockLogger.Object,
                mockMediator.Object);

            var response = new GetBackgroundSamplerServiceSamplesResponse()
            {
                Samples = new List<Dto.Response.Sample>()
                {
                    new Dto.Response.Sample()
                    {
                        Url = "http://www.pop.com"
                    }
                }
            };

            mockMediator.Setup(x => x.Send(
                It.IsAny<GetBackgroundSamplerServiceSamplesQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await sut.GetLatestRawSamples();

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var samplesResult = okObjectResult.Value as List<Dto.Response.Sample>;
            Assert.NotNull(samplesResult);
            Assert.Single(samplesResult);
        }

        [Fact]
        public async void GivenParams_WhenGetSummedGraphData_ThenRequestSent_AndResponseReturned()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ReportDataController>>();
            var mockMediator = new Mock<IMediator>();
            var sut = new ReportDataController(
                mockLogger.Object,
                mockMediator.Object);

            var query = new Dto.Request.GetSummedGraphDataQuery()
            {
                From = new System.DateTime(2020, 1, 1),
                To = new System.DateTime(2020, 1, 1, 23, 59, 59)
            };

            var response = new GetSummedGraphDataResponse()
            {
                SummedGraphData = new SummedGraphData()
                {
                    Summary = new Summary(),
                    Samples = new List<Dto.Response.Sample>()
                    {
                        new Dto.Response.Sample()
                    }
                }
            };

            mockMediator.Setup(x => x.Send(
                It.IsAny<GetSummedGraphDataQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await sut.GetSummedGraphData(query);

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var summedGraphData = okObjectResult.Value as SummedGraphData;
            Assert.NotNull(summedGraphData);
            Assert.Single(summedGraphData.Samples);
        }
    }
}
