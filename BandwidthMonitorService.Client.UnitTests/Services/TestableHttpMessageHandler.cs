using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Client.UnitTests.Services
{
    public class TestableHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request));
        }

        public virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }
    }
}
