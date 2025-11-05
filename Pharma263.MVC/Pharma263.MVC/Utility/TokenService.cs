using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Pharma263.MVC.Utility
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetToken()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString(StaticDetails.SessionToken);
        }
    }
}
