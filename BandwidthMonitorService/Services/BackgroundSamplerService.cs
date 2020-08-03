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
    public class BackgroundSamplerService : IHostedService, IBackgroundSamplerService
    {
        public event EventHandler<EventArgs> Started;
        public event EventHandler<EventArgs> Stopped;

        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;
        private ManualResetEvent _stopped;
        private readonly IAppSettings _appSettings;
        private readonly ISamplerService _samplerService;
        private readonly IAsyncDelayService _asyncDelayService;
        private readonly DownloadUrls _downloadUrls;
        private readonly ConcurrentDictionary<string, Dto.Response.Sample> _latestSamples = new ConcurrentDictionary<string, Dto.Response.Sample>();


        public BackgroundSamplerService(
            IAppSettings appSettings,
            ISamplerService samplerService,
            IAsyncDelayService asyncDelayService,
            DownloadUrls downloadUrls)
        {
            _appSettings = appSettings;
            _downloadUrls = downloadUrls;
            _stopped = new ManualResetEvent(true);
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _samplerService = samplerService;
            _asyncDelayService = asyncDelayService;
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
            _stopped.Reset();

            while (!_cancellationToken.IsCancellationRequested)
            {
                await CollectAsync(
                    _appSettings.StoreSamples,
                    _cancellationToken);
            }

            _stopped.Set();
            Stopped?.Invoke(this, EventArgs.Empty);
        }

        public async Task CollectAsync(
            bool store,
            CancellationToken cancellationToken)
        {
            var results = await _samplerService.Sample(
                _downloadUrls,
                store,
                cancellationToken);
            var successResults = results.Where(x => x.IsSuccess);
            foreach (var curResult in successResults)
            {
                _latestSamples.AddOrUpdate(
                    curResult.Sample.Url,
                    curResult.Sample,
                    (key, oldValue) => curResult.Sample);
            }

            await _asyncDelayService.Delay(
                new TimeSpan(0, _appSettings.MinutesBetweenSamples, 0),
                new TimeSpan(0, 0, 5),
                null,
                cancellationToken);
        }

        public List<Dto.Response.Sample> GetSamples()
        {
            return _latestSamples.Values.ToList();
        }
    }
}
