using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MozzieAiSystems.Dtos;
using MozzieAiSystems.Models;
using MozzieAiSystems.Utility;
using Newtonsoft.Json;

namespace MozzieAiSystems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly MozzieContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private ILog log;

        public LocationController(IHostingEnvironment hostingEnvironment, MozzieContext context, IMapper mapper, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            this.log = LogManager.GetLogger(Startup.Repository.Name, typeof(LocationController));
        }
        /// <summary>
        /// Report
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateLocation(CreateLocationRequest request)
        {
            log.Info("Begin Sending The Report");
            var location = _mapper.Map<Location>(request);
            var apiKey = _configuration.GetSection("GoogleApiKey").Value;
            //逆地址，获取坐标的位置信息
            var geoCodingAddress = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={request.Lat},{request.Lng}&key={apiKey}";
            //HttpClient client = new HttpClient();
            //var geo = await client.GetStringAsync(geoCodingAddress);
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
            var response = await SendReportToCertis(request);
            var result = await response.Content.ReadAsStringAsync();
            if (response.StatusCode!= HttpStatusCode.OK)
            {
                log.Error($"End Sending The Report To Certis and get the error:{result}");
                return StatusCode(500, result);
            }
            else
            {
                log.Info($"End Sending The Report To Certis and get the response:{result}");
            }
            log.Info("End Sending The Report");
            return Ok();
        }

        /// <summary>
        /// 发送报告给certis
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> SendReportToCertis(CreateLocationRequest request)
        {
            log.Info("Begin Sending The Report to Certis");
            var certisReportUrl = _configuration["Certis_Report_Url"];
            HttpClient client = new HttpClient();
            CertisReportModel model = new CertisReportModel();
            model.category = "MOSQUITO";
            model.subcategory = request.SubCategory;
            model.timestamp = DateTime.Now.ToString();
            model.when = request.ReportDateTime.ToString();
            model.where = request.Address;
            model.detail = request.AdditionalInfo;
            model.eventlocation[0] = request.Lat;
            model.eventlocation[1] = request.Lng;
            model.reported_by = "MOBILE";
            model.site = "CW";
            var photo = request.Files[0];
            var photoPath = Directory.GetCurrentDirectory() + photo.FilePath.Replace("/", "\\");
            //TODO:此处需要压缩图片，分辨率控制在1920*1080以下，图片要小于250kb，然后再把压缩后的图片传给接口
            CompressImage.CompressImages(photoPath);
            using (FileStream stream = new FileStream(photoPath, FileMode.Open, FileAccess.Read))
            {
                byte[] bt = new byte[stream.Length];
                //调用read读取方法
                stream.Read(bt, 0, bt.Length);
                var base64Str = Convert.ToBase64String(bt);
                model.image = base64Str;
            }
            log.Info($"The report data to Certis is:{JsonConvert.SerializeObject(model)}");
            var result = await client.PostAsJsonAsync(certisReportUrl, model);
            return result;
        }
        /// <summary>
        /// Fetch the local activities
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("activities")]
        public async Task<ActionResult<List<LocalActivityListResponse>>> GetLocalActivities(GetNearbyReportsRequest request)
        {
            if (Math.Abs(request.Lng) < 1 || Math.Abs(request.Lat) < 1)
            {
                return BadRequest(new {msg = "The longitude and latitude are required." });
            }

            var setting = _configuration.GetSection("ActivitySetting");
            var distance = 2;
            var count = 20;

            int.TryParse(setting["LocalActivityDistance"],out distance);
            int.TryParse(setting["LocalActivityCount"],out  count);

            var checkDate =
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-request.Days);

            log.Info("Begin Getting The Local Activities.");
            var locations = _context.Locations.AsQueryable();
            var list = from l in locations
                where GpsLocationHelp.GetDistance(request.Lat, request.Lng, l.Lat, l.Lng) <= distance
                      && l.ReportDateTime >= checkDate
                select new LocalActivityListResponse
                {
                    Id = l.Id,
                    Address = l.Address,
                    Lat = l.Lat,
                    Lng = l.Lng,
                    ReportDateTime = l.ReportDateTime,
                    ReportUserId = l.ReportUserId,
                    Name = l.Name,
                    Uuid = l.Uuid
                };
            var response = await list.OrderByDescending(p => p.ReportDateTime).Skip(0).Take(count).ToListAsync();
            log.Info("End Getting The Local Activities.");
            return Ok(new {result = response});
        }
        /// <summary>
        /// Get The NEA Dengue clusters
        /// </summary>
        /// <returns></returns>
        [HttpGet("dengue")]
        public async Task<ActionResult> GetNeaDengueClusters()
        {
            log.Info("Begin Getting The DengueClusters.");
            var setting = _configuration.GetSection("ActivitySetting");
            var url = setting["NEADengueApiUrl"];
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Referrer = new Uri("http://www.xinaisys.com");
            client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.22.0");
            var data = await client.GetStringAsync(url);
            data = data.Replace("\\", "").Remove(0, 1);
            data = data.Remove(data.Length - 1, 1);
            var clusters = JsonConvert.DeserializeObject<NeaDengueResponse>(data);
            foreach (var item in clusters.SrchResults)
            {
                if (!string.IsNullOrEmpty(item.LatLng))
                {
                    var pointItem = item.LatLng.Split("|");
                    foreach (var s in pointItem)
                    {
                        var latLng = s.Split(",");
                        item.Points.Add(new LatLngPoint
                        {
                            Lat = float.Parse(latLng[0]),
                            Lng = float.Parse(latLng[1])
                        });
                    }
                    
                }
            }
            log.Info("End Getting The DengueClusters.");
            return Ok(clusters);
        }

        /// <summary>
        /// Send the task to certis, and get the AI response
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("task")]
        public async Task<ActionResult<CertisAiResponse>> SendTask(SendTaskRequest request)
        {
            if (ModelState.IsValid)
            {
                log.Info($"Begin Sending The Task To Certis with the data:{JsonConvert.SerializeObject(request)}");
                var url = _configuration["Certis_Task_Url"];
                var token = _configuration["Certis_Token"];
                HttpClient client = new HttpClient();
                var formContent = new MultipartFormDataContent();
                formContent.Add(new StringContent(request.ProjectName),"projectName");
                formContent.Add(new StringContent(request.DeviceType), "deviceType");
                formContent.Add(new StringContent(request.InferenceObjectType), "inferenceObjectType");
                for (int i = 0; i < request.ImageURLs.Count; i++)
                {
                    formContent.Add(new StringContent(request.ImageURLs[i]), $"imageURLs[{i}]");
                }
                if (request.Lat.HasValue)
                {
                    formContent.Add(new StringContent(request.Lat.Value.ToString()), $"locationCoordinate[0]");
                }
                if (request.Lng.HasValue)
                {
                    formContent.Add(new StringContent(request.Lng.Value.ToString()), $"locationCoordinate[1]");
                }
                if (!string.IsNullOrEmpty(request.LocationName))
                {
                    formContent.Add(new StringContent(request.LocationName), "locationName");
                }
                client.DefaultRequestHeaders.Authorization= new AuthenticationHeaderValue("Bearer", token);
                var task = await client.PostAsync(url, formContent);
                var result = await task.Content.ReadAsStringAsync();
                if (task.StatusCode != HttpStatusCode.OK)
                {
                    log.Error($"End Sending The Task To Certis and get the error:{result}");
                    return StatusCode(500, result);
                }
                var response = JsonConvert.DeserializeObject<CertisAiResponse>(result);
                log.Info($"End Sending The Task To Certis and get the response:{result}");
                return  Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}