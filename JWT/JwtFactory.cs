using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MozzieAiSystems.Models;
using Newtonsoft.Json;

namespace MozzieAiSystems.JWT
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodeToken(string userName, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(User user);
    }

    public class JwtFactory: IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtIssuerOptions;

        public JwtFactory(IOptions<JwtIssuerOptions> options)
        {
            _jwtIssuerOptions = options.Value;
            //ThrowIfInvalidOptions(_jwtIssuerOptions);
        }

        public async Task<string> GenerateEncodeToken(string userName, ClaimsIdentity identity)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtIssuerOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, _jwtIssuerOptions.IssuedAt.ToString(),
                    ClaimValueTypes.Integer64),
                identity.FindFirst(ClaimTypes.Name),
                identity.FindFirst("id")
            };
            claims.AddRange(identity.FindAll(ClaimTypes.Role));

            //Create the JWT security token and encode it
            var jwt = new JwtSecurityToken(
                issuer: _jwtIssuerOptions.Issuer,
                audience: _jwtIssuerOptions.Audience,
                claims: claims,
                notBefore: _jwtIssuerOptions.NotBefore,
                expires: _jwtIssuerOptions.Expiration,
                signingCredentials: _jwtIssuerOptions.SigningCredentials);

            var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                auth_token = encodeJwt,
                expires_in = (int) _jwtIssuerOptions.ValidFor.TotalSeconds,
                token_type = "Bearer"
            };
            return JsonConvert.SerializeObject(response, new JsonSerializerSettings {Formatting = Formatting.None});
        }

        public ClaimsIdentity GenerateClaimsIdentity(User user)
        {
            var claimsIdentity = new ClaimsIdentity(new GenericIdentity(user.UserName,"Token"));
            claimsIdentity.AddClaim(new Claim("id", user.Id.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            foreach (var role in user.Roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return claimsIdentity;
        }
    }
}
