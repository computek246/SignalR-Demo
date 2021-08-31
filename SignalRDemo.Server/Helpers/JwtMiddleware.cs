using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SignalRDemo.Server.Entities;
using SignalRDemo.Server.Services;

namespace SignalRDemo.Server.Helpers
{
    public class JwtMiddleware
    {


        private readonly RequestDelegate _next;
        private readonly ITokenService<User> _tokenService;


        public JwtMiddleware(RequestDelegate next, ITokenService<User> tokenService)
        {
            _next = next;
            _tokenService = tokenService;
        }



        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                if (!_tokenService.ValidateJwtToken(token, context))
                    return;

            await _next(context);
        }


    }
}
