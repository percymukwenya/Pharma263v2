using Microsoft.AspNetCore.Mvc;
using Pharma263.Api.Services;
using Pharma263.Domain.Common;
using Pharma263.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("GetDashboardData")]
        public async Task<ActionResult<ApiResponse<DashboardResponseModel>>> GetDashboardData()
        {
            try
            {
                var data = await _dashboardService.GetDashboardData(10);
                return Ok(ApiResponse<DashboardResponseModel>.CreateSuccess(data, "Dashboard data retrieved successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DashboardResponseModel>.CreateFailure("Failed to retrieve dashboard data: " + ex.Message, 500));
            }
        }

        [HttpGet("GetDashboardDataWithTrends")]
        public async Task<ActionResult<ApiResponse<DashboardResponseModel>>> GetDashboardDataWithTrends([FromQuery] int days = 30)
        {
            try
            {
                var data = await _dashboardService.GetDashboardDataWithTrends(10, days);
                return Ok(ApiResponse<DashboardResponseModel>.CreateSuccess(data, "Dashboard data with trends retrieved successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DashboardResponseModel>.CreateFailure("Failed to retrieve dashboard trends: " + ex.Message, 500));
            }
        }
    }
}
