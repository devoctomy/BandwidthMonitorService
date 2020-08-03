using AutoMapper;
using BandwidthMonitorService.Domain.Settings;
using BandwidthMonitorService.DomainServices;
using BandwidthMonitorService.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;

namespace BandwidthMonitorService
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public AppSettings AppSettings { get; private set; }
        private BackgroundSamplerService BackgroundSamplerService;

        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            AppSettings = new AppSettings();
            Configuration.Bind(AppSettings);
            services.AddSingleton<IAppSettings>((serviceProvider) => AppSettings);

            services.AddSingleton<DownloadUrls>((p) =>
            {
                if(File.Exists(AppSettings.DownloadSampleUrlsFile))
                {
                    var downloadUrls = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(AppSettings.DownloadSampleUrlsFile));
                    return new DownloadUrls(downloadUrls);
                }
                else
                {
                    return new DownloadUrls();
                }
            });

            services.AddHttpClient();
            services.AddControllers();
            services.AddMvc()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Bandwidth Monitor Service Api",
                    Version = "v1"
                });
                options.DescribeAllEnumsAsStrings();
            });
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            ConfigureMongoDb(services);
            services.AddTransient<IFileDownloaderService, FileDownloaderService>();
            services.AddTransient<ITimestampService, TimestampService>();
            services.AddTransient<ISamplesService, SamplesService>();
            services.AddTransient<ISampleFrequencyRangeCheckerService, SampleFrequencyRangeCheckerService>();
            services.AddTransient<ISampleGroupingService, SampleGroupingService>();
            services.AddTransient<ISampleSummingService, SampleSummingService>();
            services.AddTransient<IPingService, PingService>();
            services.AddTransient<ISamplerService, SamplerService>();
            services.AddTransient<IAsyncDelayService, AsyncDelayService>();
            services.AddSingleton<BackgroundSamplerService>();
            services.AddSingleton<Ping>();
            services.AddSingleton<IHostedService>(p => p.GetService<BackgroundSamplerService>());
            services.AddSingleton<IBackgroundSamplerService>((p) =>
            {
                BackgroundSamplerService = p.GetService<BackgroundSamplerService>();
                return BackgroundSamplerService;
            });
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureMongoDb(IServiceCollection services)
        {
            services.Configure<SamplesDatabaseSettings>(Configuration.GetSection(nameof(SamplesDatabaseSettings)));
            services.AddSingleton<ISamplesDatabaseSettings>(sp => sp.GetRequiredService<IOptions<SamplesDatabaseSettings>>().Value);
            services.AddSingleton<SamplesService>();
        }
    }
}
