using AutoMapper;
using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.DomainServices;
using BandwidthMonitorService.Dto.Enums;
using BandwidthMonitorService.Messages;
using BandwidthMonitorService.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Xunit;

namespace BandwidthMonitorService.UnitTests.Messages
{
    public class SumSamplesHandlerTests
    {
        [Fact]
        public async void GivenQuery_WhenHandled_ThenFindSamples_AndSamplesGrouped_AndSamplesSummed_AndSamplesMappedToDto_AndDtoSamplesReturned()
        {
            // Arrange
            var mockSamplesService = new Mock<ISamplesService>();
            var mockTimestampService = new Mock<ITimestampService>();
            var mockMapper = new Mock<IMapper>();
            var mockSampleGroupingService = new Mock<ISampleGroupingService>();
            var mockSampleSummingService = new Mock<ISampleSummingService>();
            var sut = new SumSamplesHandler(
                mockSamplesService.Object,
                mockTimestampService.Object,
                mockMapper.Object,
                mockSampleGroupingService.Object,
                mockSampleSummingService.Object);

            var request = new SumSamplesQuery()
            {
                From = new DateTime(2020, 1, 1),
                To = new DateTime(2020, 1, 1, 23, 59, 59)
            };

            var samples = new List<Sample>()
            {
                new Sample()
                {
                    Id = "Hello World"
                }
            };
            var foundSamples = samples.AsEnumerable();
            var groupedSamples = new List<IGrouping<int, Sample>>();
            var samplesDomain = new List<Dto.Response.SummedSample>();

            mockTimestampService.Setup(x => x.ToUnixTimestamp(
                It.IsAny<DateTime>()))
                .Returns(0);

            mockSamplesService.Setup(x => x.Find(
                It.IsAny<Expression<Func<Sample, bool>>>()))
                .Returns(foundSamples);

            mockSampleGroupingService.Setup(x => x.Group(
                It.IsAny<List<Sample>>(),
                It.IsAny<Frequency>()))
                .Returns(groupedSamples);

            mockSampleSummingService.Setup(x => x.Sum(
                It.IsAny<List<IGrouping<int, Sample>>>(),
                It.IsAny<Frequency>(),
                It.IsAny<SummingMode>()))
                .Returns(samplesDomain);

            mockMapper.Setup(x => x.Map<List<Dto.Response.Sample>>(
                It.IsAny<object>()))
                .Returns(new List<Dto.Response.Sample>());

            // Act
            var result = await sut.Handle(
                request,
                CancellationToken.None);

            // Assert
            mockTimestampService.Verify(x => x.ToUnixTimestamp(
                It.IsAny<DateTime>()), Times.Exactly(2));

            mockSamplesService.Verify(x => x.Find(
                It.IsAny<Expression<Func<Sample, bool>>>()), Times.Once);

            mockSampleGroupingService.Verify(x => x.Group(
                It.Is<List<Sample>>(y => y.Count == 1 && y[0].Id == samples[0].Id),
                It.IsAny<Frequency>()), Times.Once);

            mockSampleSummingService.Verify(x => x.Sum(
                It.Is<List<IGrouping<int, Sample>>>(y => y == groupedSamples),
                It.IsAny<Frequency>(),
                It.IsAny<SummingMode>()), Times.Once);

            mockMapper.Verify(x => x.Map<List<Dto.Response.Sample>>(
                It.Is<object>(y => y == samplesDomain)), Times.Once);
        }
    }
}
