using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace netcore.Controllers
{
    [ApiController]
    [Route("api/nightscout")]
    public class NightscoutController : ControllerBase
    {
        private readonly ILogger<NightscoutController> _logger;

        public NightscoutController(ILogger<NightscoutController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public void Get()
        {
            _logger.LogInformation("进入nightscout,get");
            var str = Request.QueryString.Value;
            _logger.LogInformation($"QueryString:{str}");
        }
        [HttpPost]
        public void Post()
        {
            _logger.LogInformation("进入nightscout,post");
            var str = Request.QueryString.Value;
            _logger.LogInformation($"QueryString:{str}");
            StreamReader streamReader = new StreamReader(Request.Body);
            string content = streamReader.ReadToEndAsync().GetAwaiter().GetResult();
            _logger.LogInformation($"body:{content}");

           
        }
    }
}