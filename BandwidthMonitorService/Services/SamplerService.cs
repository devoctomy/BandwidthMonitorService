using AutoMapper;
using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Services
{
    public class SamplerService : ISamplerService
    {
        private readonly IAppSettings _appSettings;
        private readonly IFileDownloaderService _fileDownloaderService;
        private readonly ISamplesService _samplesService;
        private readonly IMapper _mapper;
        private readonly ITimestampService _timestampService;
        private readonly IPingService _pingService;

        public SamplerService(
            IAppSettings appSettings,
            IFileDownloaderService fileDownloaderService,
            ISamplesService samplesService,
            IMapper mapper,
            ITimestampService timestampService,
            IPingService pingService)
        {
            _appSettings = appSettings;
            _fileDownloaderService = fileDownloaderService;
            _samplesService = samplesService;
            _mapper = mapper;
            _timestampService = timestampService;
            _pingService = pingService;
        }

        public async Task<Dto.Response.Sample> Sample(
            string sampleUrl,
            CancellationToken cancellationToken)
        {
            var currentSampleTime = DateTime.Now;
            var currentSampleTimestamp = _timestampService.ToUnixTimestamp(currentSampleTime);

            var url = new Uri(sampleUrl);
            var pingResult = await _pingService.SendPingAsync(url.Host);
            if (pingResult.Status == IPStatus.Success)
            {
                var result = await _fileDownloaderService.DownloadAndDiscardAsync(
                    sampleUrl,
                    _appSettings.DownloadBufferSize,
                    cancellationToken);

                if (!cancellationToken.IsCancellationRequested)
                {
                    if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var sample = new Sample()
                        {
                            Url = sampleUrl,
                            Timestamp = currentSampleTimestamp,
                            BytesRead = result.TotalRead,
                            TotalReads = result.TotalReads,
                            Elapsed = result.Elapsed,
                            RoundTripTime = pingResult.RoundTripTime
                        };
                        _samplesService.Create(sample);

                        var sampleDto = _mapper.Map<Dto.Response.Sample>(sample);
                        return sampleDto;
                    }
                }
            }

            return null;
        }
    }
}
