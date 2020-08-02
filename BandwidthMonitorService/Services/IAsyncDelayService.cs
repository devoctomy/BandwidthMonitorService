using System;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Services
{
    public interface IAsyncDelayService
    {
        Task Delay(
            TimeSpan delay,
            TimeSpan delayBetweenChecks,
            CancellationToken cancellationToken);
    }
}
