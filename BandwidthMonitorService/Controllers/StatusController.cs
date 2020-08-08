using BandwidthMonitorService.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly ILogger<StatusController> _logger;
        private readonly IMediator _mediator;

        public StatusController(
            ILogger<StatusController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("Service")]
        public async Task<ActionResult> GetServiceStatus()
        {
            var result = await _mediator.Send(new GetServiceStatusQuery());
            return Ok(result.ServiceStatus);
        }
    }
}
