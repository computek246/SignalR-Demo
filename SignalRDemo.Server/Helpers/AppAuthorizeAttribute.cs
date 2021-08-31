using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SignalRDemo.Server.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AppAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private const string Values = "The request has not been applied because it lacks valid authentication credentials for the target resource";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User.FindFirst("id")?.Value;
            if (!string.IsNullOrEmpty(user)) return;


            // not logged in
            context.Result = new JsonResult(new { message = Values })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };

        }
    }
}
