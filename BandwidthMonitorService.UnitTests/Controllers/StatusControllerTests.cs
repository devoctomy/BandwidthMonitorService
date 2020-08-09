using BandwidthMonitorService.Controllers;
using BandwidthMonitorService.Dto.Response;
using BandwidthMonitorService.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using Xunit;

namespace BandwidthMonitorService.UnitTests.Controllers
{
    public class StatusControllerTests
    {
        [Fact]
        public async void GivenNoParams_WhenGetServiceStatus_ThenRequestSent_AndResponseReturned()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<StatusController>>();
            var mockMediator = new Mock<IMediator>();
            var sut = new StatusController(
                mockLogger.Object,
                mockMediator.Object);

            var response = new GetServiceStatusResponse()
            {
                ServiceStatus = new Dto.Response.ServiceStatus()
            };

            mockMediator.Setup(x => x.Send(
                It.IsAny<GetServiceStatusQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await sut.GetServiceStatus();

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var statusResult = okObjectResult.Value as ServiceStatus;
            Assert.NotNull(statusResult);
        }
    }
}
