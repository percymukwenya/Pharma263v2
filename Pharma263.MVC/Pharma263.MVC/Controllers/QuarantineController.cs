using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Integration.Api.Models.Common;
using Pharma263.MVC.DTOs.Quarantine;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    public class QuarantineController : BaseController
    {
        private readonly IQuarantineService _quarantineService;

        public QuarantineController(IQuarantineService quarantineService)
        {
            _quarantineService = quarantineService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetQuarantineDataTable([FromBody] DataTableRequest request)
        {
            try
            {
                var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
                var quarantineStock = await _quarantineService.GetAllAsync<List<QuarantineStockDto>>(token);

                if (quarantineStock == null)
                {
                    return Json(new DataTableResponse<QuarantineStockDto>
                    {
                        Draw = request.Draw,
                        RecordsTotal = 0,
                        RecordsFiltered = 0,
                        Data = new List<QuarantineStockDto>(),
                        Error = "Failed to load quarantine stock"
                    });
                }

                var query = quarantineStock.AsQueryable();

                // Apply search filter
                if (!string.IsNullOrEmpty(request.Search?.Value))
                {
                    var searchValue = request.Search.Value.ToLower();
                    query = query.Where(x => 
                        x.MedicineName.ToLower().Contains(searchValue) ||
                        x.BatchNo.ToLower().Contains(searchValue) ||
                        x.TotalQuantity.ToString().Contains(searchValue));
                }

                var recordsFiltered = query.Count();

                // Apply sorting
                if (request.Order != null && request.Order.Any())
                {
                    var orderColumn = request.Order.First();
                    var sortColumnName = GetSortColumn(orderColumn.Column);
                    
                    if (!string.IsNullOrEmpty(sortColumnName))
                    {
                        if (orderColumn.Dir == "desc")
                        {
                            query = sortColumnName switch
                            {
                                "medicinename" => query.OrderByDescending(x => x.MedicineName),
                                "batchno" => query.OrderByDescending(x => x.BatchNo),
                                "totalquantity" => query.OrderByDescending(x => x.TotalQuantity),
                                _ => query.OrderByDescending(x => x.Id)
                            };
                        }
                        else
                        {
                            query = sortColumnName switch
                            {
                                "medicinename" => query.OrderBy(x => x.MedicineName),
                                "batchno" => query.OrderBy(x => x.BatchNo),
                                "totalquantity" => query.OrderBy(x => x.TotalQuantity),
                                _ => query.OrderBy(x => x.Id)
                            };
                        }
                    }
                }

                // Apply pagination
                var data = query.Skip(request.Start).Take(request.Length).ToList();

                return Json(new DataTableResponse<QuarantineStockDto>
                {
                    Draw = request.Draw,
                    RecordsTotal = quarantineStock.Count,
                    RecordsFiltered = recordsFiltered,
                    Data = data
                });
            }
            catch (System.Exception ex)
            {
                return Json(new DataTableResponse<QuarantineStockDto>
                {
                    Draw = request.Draw,
                    RecordsTotal = 0,
                    RecordsFiltered = 0,
                    Data = new List<QuarantineStockDto>(),
                    Error = ex.Message
                });
            }
        }

        private string GetSortColumn(int column)
        {
            return column switch
            {
                0 => "medicinename",
                1 => "batchno",
                2 => "totalquantity",
                _ => null
            };
        }
    }
}
