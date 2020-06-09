using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCaching.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MozzieAiSystems.Dtos;
using MozzieAiSystems.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MozzieAiSystems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CmsController : ControllerBase
    {
        private readonly MozzieContext _context;
        private readonly IConfiguration _configuration;
        private ILog log;

        public CmsController(MozzieContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            this.log = LogManager.GetLogger(Startup.Repository.Name, typeof(CmsController));
        }


        [HttpGet("{type}")]
        public async Task<ActionResult<AlagattasContentEntryResponse>> GetContentEntry(string type)
        {
            var cms = _configuration.GetSection("CMS");
            var cmsTypes = _configuration.GetSection("CMS_Types");
            int typeId = 0;
            if (!string.IsNullOrEmpty(cmsTypes[type]))
            {
                int.TryParse(cmsTypes[type], out typeId);
            }

            if (typeId == 0)
            {
                log.Error($"Bad Request type the {type} for CMS api.");
                return BadRequest("Incorrect Type");
            }
            log.Info($"Begin getting the {type} content from CMS api.");
            var loginUrl = cms["LoginUrl"];
            var contentUrl =  cms["ContentEntryUrl"];
            var session = await _context.AlgattasCmsSessions.FirstOrDefaultAsync();
            HttpClient client = new HttpClient();
            var udid = string.Empty;
            var sessionId = string.Empty;
            if (session == null || session.Expires <= DateTime.Now)
            {
                //if the session is null or the session has been expired, recall the login api
                var credits = new List<KeyValuePair<string,string>>();
                credits.Add(new KeyValuePair<string, string>("clientId", cms["clientId"]));
                credits.Add(new KeyValuePair<string, string>("clientPassword", cms["clientPassword"]));
                var formContent = new FormUrlEncodedContent(credits);
                var login = await client.PostAsync(loginUrl, formContent);
                var loginResponse = await login.Content.ReadAsStringAsync();
                var obj = JObject.Parse(loginResponse) as JToken;
                var status = obj["responseStatus"].ToString();
                if (status == "0")
                {
                    var expires = obj["accessInfo"]["access"]["token"]["expires"].ToString();
                    var expiredDate = DateTime.ParseExact(expires, "dd/MM/yyyy HH:mm:ss",
                        System.Globalization.CultureInfo.GetCultureInfo("zh-SG"));
                    var tudid = obj["profile"]["UDID"].ToString();
                    var tsessionId = obj["profile"]["sessionId"].ToString();

                    _context.AlgattasCmsSessions.Add(new AlgattasCmsSession
                    {
                        SessionId = tsessionId,
                        Udid = tudid,
                        Expires = expiredDate,
                        UpdateDateTime = DateTime.Now
                    });
                    await _context.SaveChangesAsync();
                    udid = tudid;
                    sessionId = tsessionId;
                }
                else
                {
                    throw new Exception("The login is unsuccessful");
                }
            }
            else
            {
                udid = session.Udid;
                sessionId = session.SessionId;
            }
            
            var url = string.Format(contentUrl, typeId, udid);
            client.DefaultRequestHeaders.Add("Authorization",sessionId);
            var content = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<AlagattasContentEntryResponse>(content);

            log.Info($"End getting the {type} content from CMS api.");
            return Ok(result);
        }

        /// <summary>
        /// return the html page
        /// </summary>
        /// <returns></returns>
        [HttpGet("html/{type}")]
        public async Task ReturnHtml(string type)
        {
            var content = await GetContentEntry(type);
            StringBuilder htmlStringBuilder = new StringBuilder();
            htmlStringBuilder.Append("<html>");
            htmlStringBuilder.Append("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /><meta charset= \"utf-8\" name= \"viewport\" content= \"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0\"> </head>");//支持中文
            htmlStringBuilder.Append("<body>");
            htmlStringBuilder.Append("<spen style=\"font-size: 100%\">");//让字体变大
            var okObjectResult = content.Result as OkObjectResult;
            var htmlValue = okObjectResult?.Value as AlagattasContentEntryResponse;
            htmlStringBuilder.Append(htmlValue?.items[0].content);
            htmlStringBuilder.Append("</spen>");
            htmlStringBuilder.Append("</body>");
            htmlStringBuilder.Append("</html>");
            var result = htmlStringBuilder.ToString();
            var data = Encoding.UTF8.GetBytes(result);
            //if (accept.Any(x => x.MediaType == "text/html"))
            //{
            //    Response.ContentType = "text/html";
            //}
            //else
            //{
            //    Response.ContentType = "text/plain";
            //}
            Response.ContentType = "text/html";
            await Response.Body.WriteAsync(data, 0, data.Length);
        }

        /// <summary>
        /// Get The Dengue and Zika Case Info
        /// </summary>
        /// <returns></returns>
        [HttpGet("denguezika-cases")]
        public async Task<ActionResult<DengueZikaModel>> GetDengueZikaCases()
        {
            string dengueCaseUrl = "https://www.nea.gov.sg/dengue-zika/dengue/dengue-cases";
            string zikaCaseUrl = "https://www.nea.gov.sg/dengue-zika/zika/zika-cases-and-clusters";
            var result = new DengueZikaModel();

            HtmlWeb web = new HtmlWeb();
            var dengueDoc = await web.LoadFromWebAsync(dengueCaseUrl);
            var dengueTable = dengueDoc.DocumentNode.SelectSingleNode("//table[1]");
            var dengueDate = dengueTable.SelectSingleNode("//tr[1]/th[last()]")?.InnerText;
            var dengueCount = dengueTable.SelectSingleNode("//tr[2]/td[last()]")?.InnerText;

            result.DengueCaseUpdateDate = dengueDate?.Replace("\r\n", "").Trim();
            result.DengueCaseCount = dengueCount?.Replace("\r\n", "").Trim();

            var zikaDoc = await web.LoadFromWebAsync(zikaCaseUrl);
            var zikaTable = zikaDoc.DocumentNode.SelectSingleNode("//table[1]");
            var zikaDate = zikaTable.SelectSingleNode("//tr[1]/th[last()]")?.InnerText;
            var zikaCount = zikaTable.SelectSingleNode("//tr[2]/td[last()]")?.InnerText;
            result.ZikaCaseUpdateDate = zikaDate?.Replace("\r\n","").Trim();
            result.ZikaCaseCount = zikaCount?.Replace("\r\n", "").Trim();
            result.CurrentDate = DateTime.Now;
            return result;
        }
    }
}