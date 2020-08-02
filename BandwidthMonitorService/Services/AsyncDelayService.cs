using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Services
{
    public class AsyncDelayService : IAsyncDelayService
    {
        public async Task Delay(
            TimeSpan delay,
            TimeSpan delayBetweenChecks,
            CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                var stopWatch = new Stopwatch();
                stopWatch.Restart();
                while (!cancellationToken.IsCancellationRequested && stopWatch.Elapsed < delay)
                {
                    Console.WriteLine($"Waiting for {delay}, currently at {stopWatch.Elapsed}");
                    await Task.Delay(delayBetweenChecks, cancellationToken);
                }
                stopWatch.Stop();
            }
        }
    }
}
