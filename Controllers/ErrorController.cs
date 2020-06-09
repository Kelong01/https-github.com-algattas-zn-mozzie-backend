using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MozzieAiSystems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error-local-development")]
        [HttpGet]
        public IActionResult ErrorLocalDevelopment(
            [FromServices] IHostingEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException(
                    "This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
        }

        [Route("/error")]
        [HttpGet]
        public IActionResult Error() => Problem();

        [HttpGet("problem")]
        public IActionResult Problem(string detail = null,string title = null)
        {
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(detail))
            {
                return new JsonResult(new {title = "Error", detail = "服务器发生异常"});
            }
            return new JsonResult(new {title = title, detail = detail});
        }
    }
}