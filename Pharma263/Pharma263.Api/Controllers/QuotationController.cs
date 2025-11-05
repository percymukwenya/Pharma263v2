using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Api.Models.Quotation.Request;
using Pharma263.Api.Models.Quotation.Response;
using Pharma263.Api.Services;
using Pharma263.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotationController : ControllerBase
    {
        private readonly QuotationService _quotationService;

        public QuotationController(QuotationService quotationService)
        {
            _quotationService = quotationService;
        }

        [HttpGet("GetQuotations")]
        public async Task<ActionResult<List<QuotationListResponse>>> GetQuotations()
        {
            var result = await _quotationService.GetQuotations();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetQuotation")]
        public async Task<ActionResult<QuotationDetailsResponse>> GetQuotation([FromQuery] int id)
        {
            var response = await _quotationService.GetQuotation(id);

            return Ok(response);
        }

        [HttpPost("CreateQuotation")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CreateQuotation(AddQuotationRequest request)
        {
            var result = await _quotationService.AddQuotation(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("UpdateQuotation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateQuotation(UpdateQuotationRequest request)
        {
            var result = await _quotationService.UpdateQuotation(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete([FromQuery] int id)
        {
            var result = await _quotationService.DeleteQuotation(id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GenerateQuotationDoc")]
        public async Task<ActionResult> GenerateQuotationDoc([FromQuery] int quotationId)
        {
            var result = await _quotationService.GetQuotationDoc(quotationId);

            return Ok(result);
        }
    }
}
