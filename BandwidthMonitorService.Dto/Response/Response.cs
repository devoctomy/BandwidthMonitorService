using System.Net;

namespace BandwidthMonitorService.Dto.Response
{
    public class Response<T>
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public T Value { get; set; }
    }
}
