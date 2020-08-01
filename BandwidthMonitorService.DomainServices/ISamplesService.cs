using BandwidthMonitorService.Domain.Models;
using System.Collections.Generic;

namespace BandwidthMonitorService.DomainServices
{
    public interface ISamplesService
    {
        List<Sample> Get();

        Sample Get(string id);

        Sample Create(Sample sample);

        void Update(string id, Sample sample);

        void Remove(Sample sample);

        void Remove(string id);
    }
}
