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
    public class SaleStatusController : ControllerBase
    {
        private readonly SaleStatusService _saleStatusService;

        public SaleStatusController(SaleStatusService saleStatusService)
        {
            _saleStatusService = saleStatusService;
        }

        [HttpGet("GetSaleStatuses")]
        public async Task<ActionResult<ApiResponse<List<TypeStatusMethodListResponse>>>> GetSaleStatuses()
        {
            var response = await _saleStatusService.GetSaleStatuses();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetSaleStatus")]
        public async Task<ActionResult<ApiResponse<TypeStatusMethodDetailsResponse>>> GetSaleStatus([FromQuery] int id)
        {
            var response = await _saleStatusService.GetSaleStatus(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AddSaleStatus")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<int>>> AddSaleStatus(AddTypeStatusMethodModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<int>(
                    "Invalid sale status data", ModelState));
            }

            var response = await _saleStatusService.AddSaleStatus(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("UpdateSaleStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateSaleStatus(UpdateTypeStatusMethodModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid sale status data", ModelState));
            }

            var response = await _saleStatusService.UpdateSaleStatus(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("DeleteSaleStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteSaleStatus([FromQuery] int id)
        {
            var response = await _saleStatusService.DeleteSaleStatus(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}