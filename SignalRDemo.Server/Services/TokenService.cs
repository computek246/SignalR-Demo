using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SignalRDemo.Server.Entities;
using SignalRDemo.Server.Helpers;

namespace SignalRDemo.Server.Services
{
    public class TokenService<TUser> : ITokenService<TUser>
        where TUser : User
    {
        private readonly AppSettings _appSettings;

        public TokenService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public bool ValidateJwtToken(string token, HttpContext context)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };

                var validateToken = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
                context.User = validateToken;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string GenerateJwtToken(TUser user, int expireInHours = 24)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", $"{user.Id}"), 
                    new Claim("name", $"{user.FirstName} {user.LastName}")
                }),
                Expires = DateTime.UtcNow.AddHours(expireInHours),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}