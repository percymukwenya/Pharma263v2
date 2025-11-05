using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Api.Extensions;
using Pharma263.Api.Models.Customer.Request;
using Pharma263.Api.Models.Customer.Response;
using Pharma263.Api.Services;
using Pharma263.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("GetCustomers")]
        public async Task<ActionResult<ApiResponse<List<CustomerListResponse>>>> GetCustomers()
        {
            var response = await _customerService.GetCustomers();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetCustomersPaged")]
        public async Task<ActionResult<ApiResponse<PaginatedList<CustomerListResponse>>>> GetCustomersPaged([FromQuery] PagedRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<PaginatedList<CustomerListResponse>>("Invalid request parameters", ModelState, 400));
            }

            var response = await _customerService.GetCustomersPaged(request);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetCustomer")]
        public async Task<ActionResult<ApiResponse<CustomerDetailsResponse>>> GetCustomer([FromQuery] int id)
        {
            var response = await _customerService.GetCustomer(id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("CreateCustomer")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> CreateCustomer(AddCustomerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>("Validation failed", ModelState, 400));
            }

            var response = await _customerService.AddCustomer(request);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("UpdateCustomer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateCustomer(UpdateCustomerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>("Validation failed", ModelState, 400));
            }

            var response = await _customerService.UpdateCustomer(request);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApiResponse<bool>>> Delete([FromQuery] int id)
        {
            var response = await _customerService.DeleteCustomer(id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
