using AutoMapper;
using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.DomainServices;
using BandwidthMonitorService.Exceptions;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Services
{
    public class BackgroundSamplerService : IHostedService
    {
        public event EventHandler<EventArgs> Started;
        public event EventHandler<BackgroundSamplerServiceErrorEventArgs> Error;

        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;
        private readonly ManualResetEvent _stopped;
        private readonly IAppSettings _appSettings;
        private readonly List<string> _downloadUrls;
        private readonly IFileDownloaderService _fileDownloaderService;
        private readonly ConcurrentDictionary<string, Dto.Response.Sample> _latestSamples = new ConcurrentDictionary<string, Dto.Response.Sample>();
        private readonly ISamplesService _samplesService;
        private readonly IMapper _mapper;
        private readonly ITimestampService _timestampService;
        private readonly IPingService _pingService;

        public static BackgroundSamplerService Instance { get; private set; }

        public BackgroundSamplerService(
            IAppSettings appSettings,
            IFileDownloaderService fileDownloaderService,
            ISamplesService samplesService,
            IMapper mapper,
            ITimestampService timestampService,
            IPingService pingService)
        {
            _appSettings = appSettings;
            _downloadUrls = new List<string>()
            {
                _appSettings.DownloadUrlFrankfurt,
                _appSettings.DownloadUrlIreland,
                _appSettings.DownloadUrlLondon,
                _appSettings.DownloadUrlParis
            };
            _downloadUrls = _downloadUrls.Where(x => !string.IsNullOrEmpty(x)).ToList();
            _fileDownloaderService = fileDownloaderService;
            _stopped = new ManualResetEvent(true);
            _samplesService = samplesService;
            _mapper = mapper;
            _timestampService = timestampService;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _pingService = pingService;

            Instance = this;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            var backgroundTask = Task.Factory.StartNew(Run);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            _cancellationTokenSource.Cancel();
            _stopped.WaitOne();
            _stopped.Reset();
        }

        private async void Run()
        {
            Started?.Invoke(this, EventArgs.Empty);

            while (!_cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Started BackgroundSamplerService");
                await Task.Delay(new TimeSpan(0, 0, _appSettings.SecondsDelayBeforeFirstSample));
                _stopped.Reset();

                if(_downloadUrls.Count > 0)
                {
                    var latestSamples = new Dictionary<string, Sample>();
                    var currentSampleTime = DateTime.Now;
                    var currentSampleTimestamp = _timestampService.ToUnixTimestamp(currentSampleTime);
                    Console.WriteLine($"Taking sample at timestamp {currentSampleTimestamp}");

                    foreach (var curDownloadUrl in _downloadUrls)
                    {
                        var url = new Uri(curDownloadUrl);

                        Console.WriteLine($"Pinging host '{url.Host}'");
                        var pingResult = await _pingService.SendPingAsync(url.Host);
                        if(pingResult.Status == IPStatus.Success)
                        {
                            Console.WriteLine($"Time download from '{curDownloadUrl}'");

                            var result = await _fileDownloaderService.DownloadAndDiscardAsync(
                                curDownloadUrl,
                                _appSettings.DownloadBufferSize,
                                _cancellationToken);

                            if (!_cancellationToken.IsCancellationRequested)
                            {
                                if(result.HttpStatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    Console.WriteLine($"File at url '{curDownloadUrl}' took {result.Elapsed} to download. Totalling {result.TotalRead} bytes in {result.TotalReads} reads");

                                    var sample = new Sample()
                                    {
                                        Url = curDownloadUrl,
                                        Timestamp = currentSampleTimestamp,
                                        BytesRead = result.TotalRead,
                                        TotalReads = result.TotalReads,
                                        Elapsed = result.Elapsed,
                                        RoundTripTime = pingResult.RoundTripTime
                                    };
                                    _samplesService.Create(sample);

                                    Console.WriteLine("Storing sample");
                                    var sampleDto = _mapper.Map<Dto.Response.Sample>(sample);
                                    _latestSamples.AddOrUpdate(
                                        curDownloadUrl,
                                        sampleDto,
                                        (key, oldValue) => sampleDto);
                                    Console.WriteLine("Sample stored");
                                }
                                else
                                {
                                    Console.WriteLine($"Download failed '{url}'");
                                    Error?.Invoke(this, new BackgroundSamplerServiceErrorEventArgs()
                                    {
                                        Exception = new DownloadFailedException(curDownloadUrl)
                                    });
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Failed to ping host '{url.Host}'");
                            Error?.Invoke(this, new BackgroundSamplerServiceErrorEventArgs()
                            {
                                Exception = new HostPingFailedException(url.Host, pingResult.Status)
                            });
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"No download Urls configured");
                    Error?.Invoke(this, new BackgroundSamplerServiceErrorEventArgs()
                    {
                        Exception = new NoDownloadUrlsConfiguredException()
                    });
                    _cancellationTokenSource.Cancel();
                }

                if (!_cancellationToken.IsCancellationRequested)
                {
                    var stopWatch = new Stopwatch();
                    stopWatch.Restart();
                    var delay = new TimeSpan(0, _appSettings.MinutesBetweenSamples, 0);
                    while (stopWatch.Elapsed < delay)
                    {
                        Console.WriteLine($"Waiting for {delay}, currently at {stopWatch.Elapsed}");
                        await Task.Delay(new TimeSpan(0, 0, 5));
                    }
                    stopWatch.Stop();
                }
            }
            _stopped.Set();
            Console.WriteLine("Stopped BackgroundSamplerService");
        }

        public List<Dto.Response.Sample> GetSamples()
        {
            return _latestSamples.Values.ToList();
        }
    }
}
