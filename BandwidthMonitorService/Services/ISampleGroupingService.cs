using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.Dto.Enums;
using System.Collections.Generic;
using System.Linq;

namespace BandwidthMonitorService.Services
{
    public interface ISampleGroupingService
    {
        List<IGrouping<int, Sample>> Group(
            List<Sample> samples,
            Frequency frequency);
    }
}
