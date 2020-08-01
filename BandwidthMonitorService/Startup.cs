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
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;

namespace BandwidthMonitorService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private BackgroundSamplerService BackgroundSamplerService;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddSingleton<IAppSettings>((serviceProvider) =>
            {
                var appSettings = new AppSettings();
                Configuration.Bind(appSettings);
                return appSettings;
            });
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            ConfigureMongoDb(services);
            services.AddTransient<IFileDownloaderService, FileDownloaderService>();
            services.AddTransient<ITimestampService, TimestampService>();
            services.AddTransient<ISamplesService, SamplesService>();
            services.AddTransient<IPingService, PingService>();
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
