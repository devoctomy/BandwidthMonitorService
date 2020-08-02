﻿using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.Dto.Enums;
using System.Collections.Generic;

namespace BandwidthMonitorService.Services
{
    public interface ISampleGroupingService
    {
        List<Sample> Group(
            List<Sample> samples,
            Frequency frequency);
    }
}
