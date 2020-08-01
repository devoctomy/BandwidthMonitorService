using BandwidthMonitorService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BandwidthMonitorService.DomainServices
{
    public interface ISamplesService
    {
        List<Sample> Get();

        Sample Get(string id);

        IEnumerable<Sample> Find(Expression<Func<Sample, bool>> filter);

        Sample Create(Sample sample);

        void Update(string id, Sample sample);

        void Remove(Sample sample);

        void Remove(string id);
    }
}
