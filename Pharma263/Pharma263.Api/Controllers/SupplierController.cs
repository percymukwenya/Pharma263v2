using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Api.Extensions;
using Pharma263.Api.Models.Supplier.Request;
using Pharma263.Api.Models.Supplier.Response;
using Pharma263.Api.Services;
using Pharma263.Domain.Common;
using Pharma263.Domain.Interfaces.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly SupplierService _supplierService;
        private readonly ISupplierRepository _supplierRepository;

        public SupplierController(SupplierService supplierService, ISupplierRepository supplierRepository)
        {
            _supplierService = supplierService;
            _supplierRepository = supplierRepository;
        }

        [HttpGet("GetSuppliers")]
        public async Task<ActionResult<ApiResponse<List<SupplierListResponse>>>> GetSuppliers()
        {
            var response = await _supplierService.GetSuppliers();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetSuppliersPaged")]
        public async Task<ActionResult<ApiResponse<PaginatedList<SupplierListResponse>>>> GetSuppliersPaged([FromQuery] PagedRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<PaginatedList<SupplierListResponse>>("Invalid request parameters", ModelState, 400));
            }

            var response = await _supplierService.GetSuppliersPaged(request);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetSupplier")]
        public async Task<ActionResult<ApiResponse<SupplierDetailsResponse>>> GetSupplier([FromQuery] int id)
        {
            var response = await _supplierService.GetSupplier(id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("CreateSupplier")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> CreateSupplier([FromBody] AddSupplierRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>("Validation failed", ModelState, 400));
            }

            var response = await _supplierService.AddSupplier(request);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("UpdateSupplier")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateSupplier([FromBody] UpdateSupplierRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>("Validation failed", ModelState, 400));
            }

            var response = await _supplierService.UpdateSupplier(request);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApiResponse<bool>>> Delete([FromQuery] int id)
        {
            var response = await _supplierService.DeleteSupplier(id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Route("search")]
        public ActionResult Search(string query = "", int page = 1)
        {
            int pageSize = 20;
            var suppliers = _supplierRepository.SearchSuppliers(query, page, pageSize);
            return Ok(suppliers);
        }
    }
}
