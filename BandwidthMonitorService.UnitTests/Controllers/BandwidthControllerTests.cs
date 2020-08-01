using BandwidthMonitorService.Messages;
using BandwidthMonitorService.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace BandwidthMonitorService.UnitTests.Controllers
{
    public class BandwidthControllerTests
    {
        [Fact]
        public async void GivenNoParams_WhenGetLatestRawSamples_ThenRequestSent_AndResponseReturned()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<BandwidthController>>();
            var mockMediator = new Mock<IMediator>();
            var sut = new BandwidthController(
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
    }
}
