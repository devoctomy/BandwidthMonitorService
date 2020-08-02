using BandwidthMonitorService.Messages;
using BandwidthMonitorService.Services;
using Moq;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace BandwidthMonitorService.UnitTests.Messages
{
    public class GetBackgroundSamplerServiceSamplesHandlerTests
    {
        [Fact]
        public async void GivenQuery_WhenHandled_ThenGetSamples_AndSamplesReturned()
        {
            // Arrange
            var mockBackgroundSamplerService = new Mock<IBackgroundSamplerService>();
            var sut = new GetBackgroundSamplerServiceSamplesHandler(mockBackgroundSamplerService.Object);
            var request = new GetBackgroundSamplerServiceSamplesQuery();

            var samples = new List<Dto.Response.Sample>()
            {
                new Dto.Response.Sample()
            };

            mockBackgroundSamplerService.Setup(x => x.GetSamples())
                .Returns(samples);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            mockBackgroundSamplerService.Verify(x => x.GetSamples(), Times.Once);
            Assert.Single(result.Samples);
        }
    }
}
