using BandwidthMonitorService.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
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

        [HttpGet("GetLatestRawSamples")]
        public async Task<ActionResult> GetLatestRawSamples()
        {
            var request = new GetBackgroundSamplerServiceSamplesQuery();
            var result = await _mediator.Send(request);
            return Ok(result.Samples);
        }

        [HttpPost("GetSummedGraphData")]
        [ProducesResponseType(typeof(List<Dto.Response.Sample>), 200)]
        public async Task<IActionResult> GetSummedGraphData([FromQuery] Dto.Request.SumSamplesQuery query)
        {
            var request = new SumSamplesQuery()
            {
                From = DateTime.SpecifyKind(
                    query.From,
                    DateTimeKind.Utc),
                To = DateTime.SpecifyKind(
                    query.To,
                    DateTimeKind.Utc)
            };
            var result = await _mediator.Send(request);
            return Ok(result.Samples);
        }
    }
}
