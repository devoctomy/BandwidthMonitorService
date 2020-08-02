using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.Dto.Enums;
using System.Collections.Generic;

namespace BandwidthMonitorService.Services
{
    public interface ISampleFrequencyRangeCheckerService
    {
        FrequencyRange GetRange(
            List<Sample> orderedSamples,
            Frequency frequency);
    }
}
