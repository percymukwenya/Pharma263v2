using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Api.Extensions;
using Pharma263.Api.Models.Sales.Request;
using Pharma263.Api.Models.Sales.Response;
using Pharma263.Api.Services;
using Pharma263.Domain.Common;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly SalesService _salesService;

        public SaleController(SalesService salesService)
        {
            _salesService = salesService;
        }

        [HttpGet("GetSales")]
        public async Task<ActionResult<ApiResponse<List<SaleListResponse>>>> GetSales()
        {
            var result = await _salesService.GetSales();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetSalesPaged")]
        public async Task<ActionResult<ApiResponse<PaginatedList<SaleListResponse>>>> GetSalesPaged([FromQuery] PagedRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<PaginatedList<SaleListResponse>>(
                    "Invalid request parameters", ModelState));
            }

            var response = await _salesService.GetSalesPaged(request);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetSale")]
        public async Task<ActionResult<ApiResponse<SaleDetailsResponse>>> GetSale([FromQuery] int id)
        {
            var result = await _salesService.GetSale(id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("CreateSale")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<int>>> CreateSale(AddSaleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<int>(
                    "Invalid sale data", ModelState));
            }

            var result = await _salesService.AddSale(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("UpdateSale")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateSale(UpdateSaleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid sale data", ModelState));
            }

            var result = await _salesService.UpdateSale(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApiResponse<bool>>> Delete([FromQuery] int id)
        {
            var result = await _salesService.DeleteSale(id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetSaleInvoice")]
        public async Task<ActionResult> SalesInvoice([FromQuery] int saleId)
        {
            var result = await _salesService.GetSaleInvoice(saleId);

            return Ok(result);
        }
    }
}
