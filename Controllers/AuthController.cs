using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MozzieAiSystems.Dtos;
using MozzieAiSystems.JWT;
using MozzieAiSystems.Models;

namespace MozzieAiSystems.Controllers
{
    /// <summary>
    /// 认证
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtIssuerOptions;
        private readonly MozzieContext _context;

        public AuthController(IJwtFactory factory,IOptions<JwtIssuerOptions> options, MozzieContext context)
        {
            _jwtFactory = factory;
            _jwtIssuerOptions = options.Value;
            _context = context;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.UserName == request.UserName);
            if (user == null)
            {
                ModelState.AddModelError("login_failure","Invalid User Name");
                return BadRequest(ModelState);
            }

            if (!user.Password.Equals(request.Password))
            {
                ModelState.AddModelError("login_failure", "Invalid Password");
                return BadRequest(ModelState);
            }

            var claimsIdentity = _jwtFactory.GenerateClaimsIdentity(user);
            var token = _jwtFactory.GenerateEncodeToken(user.UserName, claimsIdentity);
            return new OkObjectResult(token);
        }

        [HttpGet("[action]")]
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var claimIdentity = User.Identity as ClaimsIdentity;
            return Ok(claimIdentity.Claims.ToList().Select(r => new {r.Type, r.Value}));
        }
    }
}