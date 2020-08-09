using System;
using System.Threading.Tasks;
using BandwidthMonitorService.Client.Services;
using BandwidthMonitorService.Dto.Response;
using BandwidthMonitorService.Portal.Messages;
using BandwidthMonitorService.Portal.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BandwidthMonitorService.Portal.Controllers
{
    public class StatusController : Controller
    {
        private readonly IMediator _mediator;

        public StatusController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActionResult> Index()
        {
            var response = await _mediator.Send(new GetServiceStatusQuery());
            var model = new StatusModel()
            {
                IsOnline = response.IsOnline,
                ServiceStatus = response.ServiceStatus
            };
            return View(model);
        }
    }
}
