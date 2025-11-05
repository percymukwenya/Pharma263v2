using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Api.Extensions;
using Pharma263.Api.Models.Returns.Request;
using Pharma263.Api.Models.Returns.Response;
using Pharma263.Api.Services;
using Pharma263.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnController : ControllerBase
    {
        private readonly ReturnService _returnService;

        public ReturnController(ReturnService returnService)
        {
            _returnService = returnService;
        }

        [HttpGet("GetReturns")]
        public async Task<ActionResult<ApiResponse<List<ReturnsListResponse>>>> GetReturns()
        {
            var response = await _returnService.GetReturns();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetReturnsPaged")]
        public async Task<ActionResult<ApiResponse<PaginatedList<ReturnsListResponse>>>> GetReturnsPaged([FromQuery] PagedRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<PaginatedList<ReturnsListResponse>>(
                    "Invalid request parameters", ModelState));
            }

            var response = await _returnService.GetReturnsPaged(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("CreateReturn")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<int>>> CreateReturn(ProcessReturnRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<int>(
                    "Invalid return data", ModelState));
            }

            var response = await _returnService.AddReturn(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetReturn/{id}")]
        public async Task<ActionResult<ApiResponse<ReturnsDetailResponse>>> GetReturn(int id)
        {
            var response = await _returnService.GetReturnById(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
