using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.Dto.Enums;
using System.Collections.Generic;

namespace BandwidthMonitorService.Services
{
    public interface ISampleFrequencyRangeCheckerService
    {
        FrequencyRange GetRange(
            IEnumerable<Sample> orderedSamples,
            Frequency frequency);
    }
}
