using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Api.Extensions;
using Pharma263.Api.Models.Shared;
using Pharma263.Api.Services;
using Pharma263.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseStatusController : ControllerBase
    {
        private readonly PurchaseStatusService _purchaseStatusService;

        public PurchaseStatusController(PurchaseStatusService purchaseStatusService)
        {
            _purchaseStatusService = purchaseStatusService;
        }

        [HttpGet("GetPurchaseStatuses")]
        public async Task<ActionResult<ApiResponse<List<TypeStatusMethodListResponse>>>> GetPurchaseStatuses()
        {
            var response = await _purchaseStatusService.GetPurchaseStatuses();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetPurchaseStatus")]
        public async Task<ActionResult<ApiResponse<TypeStatusMethodDetailsResponse>>> GetPurchaseStatus([FromQuery] int id)
        {
            var response = await _purchaseStatusService.GetPurchaseStatus(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AddPurchaseStatus")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<int>>> AddPurchaseStatus(AddTypeStatusMethodModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<int>(
                    "Invalid purchase status data", ModelState));
            }

            var response = await _purchaseStatusService.AddPurchaseStatus(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("UpdatePurchaseStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> UpdatePurchaseStatus(UpdateTypeStatusMethodModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid purchase status data", ModelState));
            }

            var response = await _purchaseStatusService.UpdatePurchaseStatus(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("DeletePurchaseStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeletePurchaseStatus([FromQuery] int id)
        {
            var response = await _purchaseStatusService.DeletePurchaseStatus(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}