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
            _logger.LogInformation($"Body:{content}");
            if (content == null)
                return;
            var ls = content.Split('\n');
            var pushUrl = configuration.GetValue<string>("PushUrl").ToString();
            var frontPage = configuration.GetValue<string>("FrontPage").ToString();
            var httpClient = HttpClientFactory.Create();


            var resBody = httpClient.GetAsync($"{pushUrl}&cardMsg.first={(ls.Length > 0 ? ls[0] : "")}&cardMsg.keyword1={(ls.Length > 1 ? ls[1] : "")}&cardMsg.keyword2={(ls.Length > 2 ? ls[2] : "")}&cardMsg.remark={DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}&cardMsg.url={frontPage}").GetAwaiter().GetResult().Content.ReadAsStream();

            StreamReader resReader = new StreamReader(resBody);
            string res = resReader.ReadToEndAsync().GetAwaiter().GetResult();
            _logger.LogInformation($"res:{res}");





        }
    }
}