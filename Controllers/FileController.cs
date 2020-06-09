using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using MozzieAiSystems.Azure;
using MozzieAiSystems.Dtos;
using Newtonsoft.Json;

namespace MozzieAiSystems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IAzureBlobStorage _azureBlobStorage;
        private ILog log;
        public FileController(IHostingEnvironment hostingEnvironment, IAzureBlobStorage azureBlobStorage)
        {
            _hostingEnvironment = hostingEnvironment;
            _azureBlobStorage = azureBlobStorage;
            this.log = LogManager.GetLogger(Startup.Repository.Name, typeof(FileController));
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("{inferenceType}")]
        public async Task<ActionResult<AbpResponse>> UploadFile(string inferenceType,[FromForm] IFormCollection formCollection)
        {
            var resp = new AbpResponse();
            string result = "";
            string webRootPath = Directory.GetCurrentDirectory()+ "\\photos\\";//_hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            FormFileCollection filelist = (FormFileCollection)formCollection.Files;
            var response = new List<AttachmentModel>();
            log.Info("Begin uploading the file.");
            foreach (IFormFile file in filelist)
            {
                String Tpath = DateTime.Now.ToString("yyyy-MM-dd");
                string name = file.FileName;
                string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string FilePath = Path.Combine(webRootPath, Tpath); //webRootPath + Tpath;
                string type = Path.GetExtension(name);
                DirectoryInfo di = new DirectoryInfo(FilePath);

                if (!di.Exists) { di.Create(); }

                var path = Path.Combine(FilePath, FileName + type);

                AttachmentModel dto = new AttachmentModel();
                using (FileStream fs = System.IO.File.Create(path))
                {
                    //复制文件
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }

                //Upload file to azure blob storage
                var url = string.Empty;
                try
                {
                    url = await _azureBlobStorage.UploadSasAsync(inferenceType, Path.GetFileName(path), path);
                }
                catch (Exception ex)
                {
                    log.Error($"Error uploading file to Azure", ex);
                    return StatusCode(500, ex.Message);
                }

                dto.FileName = Path.GetFileNameWithoutExtension(name);
                dto.FileExtenstion = type;
                dto.FilePath = "/photos/" + Tpath + "/" + FileName + type;
                dto.FileSize = file.Length / 1024;
                dto.AzureBlobAddress = url;
                response.Add(dto);
            }
            log.Info("End uploading the file.");
            resp.Success = true;
            resp.Result = response;
            return resp;
        }

        //private AttachmentModel Upload(MultipartFileData data, string privateUploadPath, string myData)
        //{
        //    AttachmentModel dto = new AttachmentModel();
        //    var dbPath = "";
        //    string originalFileName = GetDeserializedFileName(data);
        //    var docKey = Guid.NewGuid().ToString();
        //    var extension = Path.GetExtension(originalFileName);
        //    var newName = docKey + extension;
        //    var targetFileName = Path.Combine(privateUploadPath, newName);
        //    File.Move(data.LocalFileName, targetFileName);
        //    dbPath = uploadPathPrefix + newName;
        //    var uploadedFileInfo = new FileInfo(targetFileName);

        //    dto.FileName = Path.GetFileNameWithoutExtension(originalFileName);
        //    dto.FileExtenstion = extension;
        //    dto.FilePath = dbPath;
        //    dto.FileSize = uploadedFileInfo.Length / 1024;
        //    return dto;
        //}

     
    }
}