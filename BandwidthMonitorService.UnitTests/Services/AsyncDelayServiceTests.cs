using BandwidthMonitorService.Services;
using System;
using System.Diagnostics;
using System.Threading;
using Xunit;

namespace BandwidthMonitorService.UnitTests.Services
{
    public class AsyncDelayServiceTests
    {
        [Fact]
        public async void GivenDelay_AndDelayPause_WhenDelay_ThenDelayIntroduced_AndCallbackInvoked()
        {
            // Arrange
            var delay = new TimeSpan(0, 0, 5);
            var delayBetweenChecks = new TimeSpan(0, 0, 1);
            var sut = new AsyncDelayService();

            var callbacks = 0;
            var callback = (Action)delegate()
            {
                callbacks += 1;
            };

            var stopWatch = new Stopwatch();

            // Act
            stopWatch.Restart();
            await sut.Delay(
                delay,
                delayBetweenChecks,
                callback,
                CancellationToken.None);
            stopWatch.Stop();

            // Assert
            Assert.Equal(5, callbacks);
            Assert.True(stopWatch.Elapsed.TotalSeconds >= delay.Seconds);
        }
    }
}
