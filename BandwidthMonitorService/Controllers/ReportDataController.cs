using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using BandwidthMonitorService.Messages;

namespace BandwidthMonitorService.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ReportDataController : ControllerBase
    {
        private readonly ILogger<ReportDataController> _logger;
        private readonly IMediator _mediator;

        public ReportDataController(
            ILogger<ReportDataController> logger,
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
        [ProducesResponseType(typeof(Dto.Request.GetSummedGraphDataQuery), 200)]
        public async Task<IActionResult> GetSummedGraphData([FromQuery] Dto.Request.GetSummedGraphDataQuery query)
        {
            var request = new GetSummedGraphDataQuery()
            {
                From = DateTime.SpecifyKind(
                    query.From,
                    DateTimeKind.Utc),
                To = DateTime.SpecifyKind(
                    query.To,
                    DateTimeKind.Utc)
            };
            var result = await _mediator.Send(request);
            return Ok(result.SummedGraphData);
        }
    }
}
