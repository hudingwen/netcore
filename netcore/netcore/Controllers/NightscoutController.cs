using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
            var nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _logger.LogInformation($"{nowTime}-进入nightscout,post");
            var str = Request.QueryString.Value;
            _logger.LogInformation($"{nowTime}-QueryString:{str}");
            StreamReader streamReader = new StreamReader(Request.Body);
            string content = streamReader.ReadToEndAsync().GetAwaiter().GetResult();
            _logger.LogInformation($"{nowTime}-Body:{content}");
            var data = JsonConvert.DeserializeObject<IFTTT>(content);
            if (data == null)
                return;
            var ls = data.Value2.Split("\n");
            if(ls.Length ==2)
            {
                data.Value2_1 = ls[0].Replace("BG Now: ","");
                data.Value2_2 = ls[1].Replace("BG 15m: ", "");
            }
            var pushUrl = configuration.GetValue<string>("PushUrl").ToString();
            var frontPage = configuration.GetValue<string>("FrontPage").ToString();
            var httpClient = HttpClientFactory.Create();

            if ("bwp".Equals(data.Value3))
                return;
            var url = $"{pushUrl}&cardMsg.first={data.Value1}&cardMsg.keyword1={data.Value2_1}&cardMsg.keyword2={data.Value2_2}&cardMsg.remark={nowTime}&cardMsg.url={frontPage}";
            _logger.LogInformation($"{nowTime}-url:{url}");
            var resBody = httpClient.GetAsync(url).GetAwaiter().GetResult().Content.ReadAsStream();

            StreamReader resReader = new StreamReader(resBody);
            string res = resReader.ReadToEndAsync().GetAwaiter().GetResult();
            _logger.LogInformation($"res:{res}");
        }
    }
    public class IFTTT
    {
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value2_1 { get; set; }
        public string Value2_2 { get; set; }
        public string Value3 { get; set; }
    }
}