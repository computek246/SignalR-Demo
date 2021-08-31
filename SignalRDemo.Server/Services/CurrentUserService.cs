using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SignalRDemo.Server.Helpers;

namespace SignalRDemo.Server.Services
{


    public class CurrentUserService<TKey> : ICurrentUserService<TKey>
    {

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var claimsPrincipal = httpContextAccessor?.HttpContext?.User;
            if (claimsPrincipal == null) return;

            var userIdValue = claimsPrincipal.FindFirstValue("id");
            if (string.IsNullOrWhiteSpace(userIdValue)) return;

            UserId = userIdValue.To<TKey>();
            FullName = claimsPrincipal.FindFirstValue("name");
            IsAuthenticated = true;
        }


        public TKey UserId { get; }

        public string FullName { get; set; }

        public bool IsAuthenticated { get; }

    }
}