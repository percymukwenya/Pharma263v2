using Microsoft.AspNetCore.Mvc;
using Pharma263.Api.Extensions;
using Pharma263.Api.Models.Quarantine;
using Pharma263.Api.Services;
using Pharma263.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuarantineStockController : ControllerBase
    {
        private readonly QuarantineService _quarantineService;

        public QuarantineStockController(QuarantineService quarantineService)
        {
            _quarantineService = quarantineService;
        }

        [HttpGet("GetQuarantineStockList")]
        public async Task<ActionResult<ApiResponse<List<QuarantineStockModel>>>> GetQuarantineStockList()
        {
            var response = await _quarantineService.GetQuarantineStocks();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetQuarantineStocksPaged")]
        public async Task<ActionResult<ApiResponse<PaginatedList<QuarantineStockModel>>>> GetQuarantineStocksPaged([FromQuery] PagedRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<PaginatedList<QuarantineStockModel>>(
                    "Invalid request parameters", ModelState));
            }

            var response = await _quarantineService.GetQuarantineStocksPaged(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetQuarantineStock")]
        public async Task<ActionResult<ApiResponse<QuarantineStockModel>>> GetQuarantineStock([FromQuery] int id)
        {
            var response = await _quarantineService.GetQuarantineStock(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
