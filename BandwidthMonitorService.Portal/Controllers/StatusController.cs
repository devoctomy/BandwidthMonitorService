using System;
using BandwidthMonitorService.Client.Services;
using BandwidthMonitorService.Dto.Response;
using BandwidthMonitorService.Portal.Models;
using Microsoft.AspNetCore.Mvc;

namespace BandwidthMonitorService.Portal.Controllers
{
    public class StatusController : Controller
    {
        private readonly IStatusClient _statusClient;

        public StatusController(IStatusClient statusClient)
        {
            _statusClient = statusClient;
        }

        public IActionResult Index()
        {
            var model = new StatusModel()
            {
                ServiceStatus = new ServiceStatus()
                {
                    Uptime = new TimeSpan(1, 1, 1)
                }
            };
            return View(model);
        }
    }
}
