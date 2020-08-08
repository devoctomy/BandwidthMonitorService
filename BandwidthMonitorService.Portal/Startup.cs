using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BandwidthMonitorService.Client.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BandwidthMonitorService.Portal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public AppSettings AppSettings { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            AppSettings = new AppSettings();
            Configuration.Bind(AppSettings);
            services.AddSingleton<IAppSettings>((serviceProvider) => AppSettings);
            services.AddControllersWithViews();
            services.AddBandwidthMonitorClients(new Client.BandwidthMonitorClientSettings()
            {
                BaseUrl = AppSettings.BandwidthMonitorServiceBaseUrl
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
