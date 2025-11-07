using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pharma263.MVC.Utility;

namespace Pharma263.MVC.Controllers
{
    /// <summary>
    /// Base controller that validates JWT token existence before executing actions.
    /// Ensures authenticated users with expired sessions are redirected to login.
    /// </summary>
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Skip validation for Auth controller actions
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            if (controllerName == "Auth")
            {
                return;
            }

            // Check if user is authenticated
            if (User.Identity?.IsAuthenticated == true)
            {
                // Validate that JWT token exists in session
                var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

                if (string.IsNullOrEmpty(token))
                {
                    // Token is missing but cookie auth is still valid
                    // This happens when session expires before cookie auth
                    // Clear authentication and redirect to login

                    // For AJAX requests, return 401 Unauthorized
                    if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }

                    // For regular requests, redirect to login
                    TempData["error"] = "Your session has expired. Please login again.";
                    context.Result = new RedirectToActionResult("Login", "Auth", null);
                }
            }
        }
    }
}
