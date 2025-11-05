using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Api.Extensions;
using Pharma263.Api.Models.StoreSettings.Request;
using Pharma263.Api.Models.StoreSettings.Response;
using Pharma263.Api.Services;
using Pharma263.Domain.Common;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreSettingsController : ControllerBase
    {
        private readonly StoreSettingsService _storeSettingsService;

        public StoreSettingsController(StoreSettingsService storeSettingsService)
        {
            _storeSettingsService = storeSettingsService;
        }

        [HttpGet("GetStoreSetting")]
        public async Task<ActionResult<ApiResponse<StoreSettingDetailsResponse>>> GetStoreSetting()
        {
            var response = await _storeSettingsService.GetStoreSetting();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("UpdateStoreSetting")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateStoreSetting(UpdateStoreSettingsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid store settings data", ModelState));
            }

            var result = await _storeSettingsService.UpdateStoreSettings(request);

            return StatusCode(result.StatusCode, result);
        }
    }
}
