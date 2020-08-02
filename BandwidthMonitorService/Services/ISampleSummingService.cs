using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.Dto.Enums;
using System.Collections.Generic;
using System.Linq;

namespace BandwidthMonitorService.Services
{
    public interface ISampleSummingService
    {
        public List<Sample> Sum(
            List<IGrouping<int ,Sample>> groupedSamples,
            SummingMode summingMode);
    }
}
