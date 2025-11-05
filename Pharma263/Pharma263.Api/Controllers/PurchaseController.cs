using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Api.Extensions;
using Pharma263.Api.Models.Purchase.Request;
using Pharma263.Api.Models.Purchase.Response;
using Pharma263.Api.Services;
using Pharma263.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseService _purchaseService;

        public PurchaseController(PurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpGet("GetPurchases")]
        public async Task<ActionResult<ApiResponse<List<PurchaseListResponse>>>> GetPurchases()
        {
            var response = await _purchaseService.GetPurchases();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetPurchasesPaged")]
        public async Task<ActionResult<ApiResponse<PaginatedList<PurchaseListResponse>>>> GetPurchasesPaged([FromQuery] PagedRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<PaginatedList<PurchaseListResponse>>(
                    "Invalid request parameters", ModelState));
            }

            var response = await _purchaseService.GetPurchasesPaged(request);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetPurchase")]
        public async Task<ActionResult<PurchaseDetailsResponse>> GetPurchase([FromQuery] int id)
        {
            var response = await _purchaseService.GetPurchase(id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("CreatePurchase")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> CreatePurchase(AddPurchaseRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid purchase data", ModelState));
            }

            var response = await _purchaseService.AddPurchase(request);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("UpdatePurchase")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApiResponse<bool>>> UpdatePurchase(UpdatePurchaseRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid purchase data", ModelState));
            }

            var response = await _purchaseService.UpdatePurchase(request);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApiResponse<bool>>> Delete([FromQuery] int id)
        {
            var response = await _purchaseService.DeletePurchase(id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetPurchaseInvoice")]
        public async Task<ActionResult> PurchaseInvoice([FromQuery] int purchaseId)
        {
            var result = await _purchaseService.GetPurchaseInvoice(purchaseId);

            return Ok(result);
        }
    }
}
