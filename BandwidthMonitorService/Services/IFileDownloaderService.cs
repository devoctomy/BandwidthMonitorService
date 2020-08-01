using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Services
{
    public interface IFileDownloaderService
    {
        public Task<DownloadResult> DownloadAndDiscardAsync(
            string url,
            int bufferSize,
            CancellationToken cancellationToken);
    }
}
