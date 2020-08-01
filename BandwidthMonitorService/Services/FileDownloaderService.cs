using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Services
{
    public class FileDownloaderService : IFileDownloaderService
    {
        private readonly HttpClient _httpClient;

        public FileDownloaderService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<DownloadResult> DownloadAndDiscardAsync(
            string url,
            int bufferSize,
            CancellationToken cancellationToken)
        {
            var result = new DownloadResult();
            var stopWatch = new Stopwatch();
            stopWatch.Restart();
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (var response = await _httpClient.SendAsync(request, cancellationToken))
                {
                    result.HttpStatusCode = response.StatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        using (var contentStream = await response.Content.ReadAsStreamAsync())
                        {
                            var buffer = new byte[bufferSize];
                            var isMoreToRead = true;
                            do
                            {
                                var read = await contentStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                                if (read == 0)
                                {
                                    isMoreToRead = false;
                                }
                                else
                                {
                                    result.TotalRead += read;
                                    result.TotalReads += 1;
                                }
                            }
                            while (isMoreToRead && !cancellationToken.IsCancellationRequested);
                        }
                    }
                }
            }
            stopWatch.Stop();
            result.Elapsed = stopWatch.Elapsed;

            return result;
        }
    }
}
