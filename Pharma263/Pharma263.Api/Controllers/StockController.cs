using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Api.Extensions;
using Pharma263.Api.Models.Stocks.Request;
using Pharma263.Api.Models.Stocks.Response;
using Pharma263.Api.Services;
using Pharma263.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly StockService _stockService;

        public StockController(StockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("GetStockList")]
        public async Task<ActionResult<ApiResponse<List<StockListResponse>>>> GetStockList()
        {
            var response = await _stockService.GetStocks();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetStocksPaged")]
        public async Task<ActionResult<ApiResponse<PaginatedList<StockListResponse>>>> GetStocksPaged([FromQuery] PagedRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<PaginatedList<StockListResponse>>(
                    "Invalid request parameters", ModelState));
            }

            var response = await _stockService.GetStocksPaged(request);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetStockItem")]
        public async Task<ActionResult<ApiResponse<StockDetailsResponse>>> GetStockItem([FromQuery] int id)
        {
            var response = await _stockService.GetStock(id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AddStock")]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApiResponse<bool>>> AddStock(AddStockRequest stock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid stock data", ModelState));
            }

            var result = await _stockService.AddStock(stock);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("AddStockBatch")]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApiResponse<BatchImportResult>>> AddStockBatch(List<AddStockRequest> stock)
        {
            var result = await _stockService.AddStockBatch(stock);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("UpdateStock")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateStock(UpdateStockRequest stock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid stock data", ModelState));
            }

            var result = await _stockService.UpdateStock(stock);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("import")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApiResponse<BatchImportResult>>> ImportStockFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            using var stream = file.OpenReadStream();

            var result = await _stockService.AddStockFromExcel(stream);

            return StatusCode(result.StatusCode, result);
        }
    }
}
