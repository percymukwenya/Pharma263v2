using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace Pharma263.MVC.Utility
{
    public class JwtTokenHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;
        private readonly IOptions<ApiSettings> _apiSettings;

        public JwtTokenHandler(ITokenService tokenService, IOptions<ApiSettings> apiSettings)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _apiSettings = apiSettings ?? throw new ArgumentNullException(nameof(apiSettings));
        }
    }
}
