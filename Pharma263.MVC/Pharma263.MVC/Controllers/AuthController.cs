using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.MVC.DTOs.Auth;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var response = await _authService.Login(request);

            if (response != null && response.Success)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(response.Data.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                var nameClaim = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.UniqueName);
                var roleClaim = jwt.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Role);
                var uidClaim = jwt.Claims.FirstOrDefault(u => u.Type == "uid");
                var emailClaim = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email);

                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
                identity.AddClaim(new Claim(CustomClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value));

                if (uidClaim != null)
                {
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, uidClaim.Value));
                    identity.AddClaim(new Claim("uid", uidClaim.Value)); // Keep the original claim name too
                }

                // Add email claim
                if (emailClaim != null)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Email, emailClaim.Value));
                }

                foreach (var claim in jwt.Claims)
                {
                    // Skip claims we've already added
                    if (claim.Type == JwtRegisteredClaimNames.UniqueName ||
                        claim.Type == ClaimTypes.Role ||
                        claim.Type == "uid" ||
                        claim.Type == JwtRegisteredClaimNames.Email)
                        continue;

                    identity.AddClaim(new Claim(claim.Type, claim.Value));
                }

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString(StaticDetails.SessionToken, response.Data.Token);

                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                TempData["error"] = response.Message;

                return View(request);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            SignOut("Cookies", "oidc");
            HttpContext.Session.SetString(StaticDetails.SessionToken, "");
            return RedirectToAction("Login", "Auth");
        }
    }
}
