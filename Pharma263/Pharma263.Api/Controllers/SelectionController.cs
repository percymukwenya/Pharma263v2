using Microsoft.AspNetCore.Mvc;
using Pharma263.Api.Models;
using Pharma263.Api.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectionController : ControllerBase
    {
        private readonly SelectionsService _selectionService;

        public SelectionController(SelectionsService selectionService)
        {
            _selectionService = selectionService;
        }

        [HttpGet("GetCustomerTypes")]
        public async Task<ActionResult<List<SelectionDto>>> GetCustomerTypes()
        {
            var customerTypes = await _selectionService.GetCustomerTypes();

            return Ok(customerTypes);
        }

        [HttpGet("GetCustomers")]
        public async Task<ActionResult<List<SelectionDto>>> GetCustomers()
        {
            var customers = await _selectionService.GetCustomers();

            return Ok(customers);
        }

        [HttpGet("GetSuppliers")]
        public async Task<ActionResult<List<SelectionDto>>> GetSuppliers()
        {
            var suppliers = await _selectionService.GetSuppliers();

            return Ok(suppliers);
        }

        [HttpGet("GetMedicines")]
        public async Task<ActionResult<List<SelectionDto>>> GetMedicines()
        {
            var medicines = await _selectionService.GetMedicines();

            return Ok(medicines);
        }

        [HttpGet("GetStocks")]
        public async Task<ActionResult<List<SelectionDto>>> GetStocks()
        {
            var stocks = await _selectionService.GetStocks();

            return Ok(stocks);
        }

        [HttpGet("GetPaymentMethods")]
        public async Task<ActionResult<List<SelectionDto>>> GetPaymentMethods()
        {
            return Ok(await _selectionService.GetPaymentMethods());
        }

        [HttpGet("GetPurchaseStatuses")]
        public async Task<ActionResult<List<SelectionDto>>> GetPurchaseStatuses()
        {
            return Ok(await _selectionService.GetPurchaseStatuses());
        }

        [HttpGet("GetSaleStatuses")]
        public async Task<ActionResult<List<SelectionDto>>> GetSaleStatuses()
        {
            return Ok(await _selectionService.GetSaleStatuses());
        }

        [HttpGet("GetReturnReasons")]
        public async Task<ActionResult<List<SelectionDto>>> GetReturnReasons()
        {
            return Ok(await _selectionService.GetReturnReasons());
        }

        [HttpGet("GetReturnDestinations")]
        public async Task<ActionResult<List<SelectionDto>>> GetReturnDestinations()
        {
            return Ok(await _selectionService.GetReturnDestinations());
        }
    }
}
