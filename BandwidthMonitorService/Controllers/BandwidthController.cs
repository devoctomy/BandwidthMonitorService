using BandwidthMonitorService.Dto.Request;
using BandwidthMonitorService.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BandwidthController : ControllerBase
    {
        private readonly ILogger<BandwidthController> _logger;
        private readonly IMediator _mediator;

        public BandwidthController(
            ILogger<BandwidthController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("LatestRawSamples")]
        public async Task<ActionResult> GetLatestRawSamples()
        {
            var request = new GetBackgroundSamplerServiceSamplesQuery();
            var result = await _mediator.Send(request);
            return Ok(result.Samples);
        }

        [HttpGet("SumSamples")]
        public async Task<ActionResult> SumSamples(Dto.Request.SumSamplesQuery query)
        {
            var request = new Messages.SumSamplesQuery()
            {
                From = query.From,
                To = query.To
            };
            var result = await _mediator.Send(request);
            return Ok(result.Samples);
        }
    }
}
