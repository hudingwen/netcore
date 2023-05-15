using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace netcore.Controllers
{
    [ApiController]
    [Route("api/nightscout")]
    public class NightscoutController : ControllerBase
    {
        private readonly ILogger<NightscoutController> _logger;
        private readonly IConfiguration configuration;

        public NightscoutController(ILogger<NightscoutController> logger, IConfiguration _configuration)
        {
            _logger = logger;
            configuration = _configuration;
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
            if(content != null )
            {
                content = content.Replace("\n", "");
            }
            _logger.LogInformation($"body:{content}");

            var pushUrl = configuration.GetValue<string>("PushUrl").ToString();
            var httpClient = HttpClientFactory.Create();

            var resBody = httpClient.GetAsync($"{pushUrl}{content}").GetAwaiter().GetResult().Content.ReadAsStream();

            StreamReader resReader = new StreamReader(resBody);
            string res = resReader.ReadToEndAsync().GetAwaiter().GetResult();
            _logger.LogInformation($"res:{res}");





        }
    }
}