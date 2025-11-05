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
    public class PaymentMethodController : ControllerBase
    {
        private readonly PaymentMethodService _paymentMethodService;

        public PaymentMethodController(PaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        [HttpGet("GetPaymentMethods")]
        public async Task<ActionResult<ApiResponse<List<TypeStatusMethodListResponse>>>> GetPaymentMethods()
        {
            var response = await _paymentMethodService.GetPaymentMethods();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetPaymentMethod")]
        public async Task<ActionResult<ApiResponse<TypeStatusMethodDetailsResponse>>> GetPaymentMethod([FromQuery] int id)
        {
            var response = await _paymentMethodService.GetPaymentMethod(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AddPaymentMethod")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<int>>> AddPaymentMethod(AddTypeStatusMethodModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<int>(
                    "Invalid payment method data", ModelState));
            }

            var response = await _paymentMethodService.AddPaymentMethod(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("UpdatePaymentMethod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> UpdatePaymentMethod(UpdateTypeStatusMethodModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid payment method data", ModelState));
            }

            var response = await _paymentMethodService.UpdatePaymentMethod(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("DeletePaymentMethod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<bool>>> DeletePaymentMethod([FromQuery] int id)
        {
            var response = await _paymentMethodService.DeletePaymentMethod(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}