
using Microsoft.AspNetCore.Http;
using Pharma263.MVC.Utility;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Middleware
{
    public class RoleMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.User;

            if (user.Identity.IsAuthenticated)
            {
                var roles = user.Claims.Where(c => c.Type == CustomClaimTypes.Role).Select(c => c.Value).ToList();
                context.Items["UserRoles"] = roles;
            }

            await _next(context);
        }
    }
}
