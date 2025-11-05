using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Api.Extensions;
using Pharma263.Api.Models.Medicine.Request;
using Pharma263.Api.Models.Medicine.Response;
using Pharma263.Api.Services;
using Pharma263.Domain.Common;
using Pharma263.Domain.Interfaces.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly MedicineService _medicineService;
        private readonly IMedicineRepository _medicineRepository;

        public MedicineController(MedicineService medicineService, IMedicineRepository medicineRepository)
        {
            _medicineService = medicineService;
            _medicineRepository = medicineRepository;
        }

        [HttpGet("GetMedicines")]
        public async Task<ActionResult<ApiResponse<List<MedicineListResponse>>>> GetMedicines()
        {
            var response = await _medicineService.GetMedicines();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetMedicinesPaged")]
        public async Task<ActionResult<ApiResponse<PaginatedList<MedicineListResponse>>>> GetMedicinesPaged([FromQuery] PagedRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<PaginatedList<MedicineListResponse>>("Invalid request parameters", ModelState, 400));
            }

            var response = await _medicineService.GetMedicinesPaged(request);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Route("GetMedicine")]
        public async Task<ActionResult<ApiResponse<MedicineDetailsResponse>>> GetMedicine([FromQuery] int id)
        {
            var response = await _medicineService.GetMedicine(id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("CreateMedicine")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> CreateMedicine(AddMedicineRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>("Validation failed", ModelState, 400));
            }

            var response = await _medicineService.AddMedicine(request);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("UpdateMedicine")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateMedicine(UpdateMedicineRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>("Validation failed", ModelState, 400));
            }

            var response = await _medicineService.UpdateMedicine(request);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ApiResponse<bool>>> Delete([FromQuery] int id)
        {
            var response = await _medicineService.DeleteMedicine(id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Route("api/medicines/search")]
        public ActionResult SearchMedicines(string query, int page = 1)
        {
            int pageSize = 20;

            var medicines = _medicineRepository.SearchMedicines(query, page, pageSize);

            return Ok(medicines);
        }
    }
}
