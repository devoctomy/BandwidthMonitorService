using BandwidthMonitorService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BandwidthMonitorService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BandwidthController : ControllerBase
    {
        private readonly ILogger<BandwidthController> _logger;

        public BandwidthController(ILogger<BandwidthController> logger)
        {
            _logger = logger;
        }

        [HttpGet("LatestRawSamples")]
        public ActionResult GetLatestRawSamples()
        {
            var samples = BackgroundSamplerService.Instance.GetSamples();
            return Ok(samples);
        }
    }
}
