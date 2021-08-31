using Microsoft.AspNetCore.Http;

namespace SignalRDemo.Server.Services
{
    public interface ITokenService<in TUser>
    {
        public bool ValidateJwtToken(string token, HttpContext context);
        public string GenerateJwtToken(TUser user, int expireInHours = 24);
    }
}
