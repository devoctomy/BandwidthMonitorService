using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.Dto.Enums;
using System.Collections.Generic;
using System.Linq;

namespace BandwidthMonitorService.Services
{
    public interface ISampleSummingService
    {
        public List<Dto.Response.SummedSample> Sum(
            List<IGrouping<int , Sample>> groupedSamples,
            Frequency freuency,
            SummingMode summingMode);
    }
}
