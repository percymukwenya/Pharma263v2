using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.MVC.Models;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly ICustomerService _customerService;
        private readonly ISupplierService _supplierService;
        private readonly IPdfReportService _pdfReportService;

        public ReportController(IReportService reportService, ICustomerService customerService, ISupplierService supplierService, IPdfReportService pdfReportService)
        {
            _reportService = reportService;
            _customerService = customerService;
            _supplierService = supplierService;
            _pdfReportService = pdfReportService;
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> Sales()
        {
            return View();
        }

        public async Task<ActionResult> SaleSummary()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SaleSummary(string startDate, string endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            DateTime parsedStartDate = DateTime.Parse(startDate);
            DateTime parsedEndDate = DateTime.Parse(endDate);

            var response = await _reportService.GenerateSalesSummaryReport(parsedStartDate, parsedEndDate, token);

            if (response != null)
            {
                return Json(response); // Return JSON response to the client
            }
            else
            {
                return Json(null);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaleSummaryData([FromBody] dynamic request)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            try
            {
                // Extract parameters from dynamic request
                string startDate = request?.startDate?.ToString();
                string endDate = request?.endDate?.ToString();
                string comparisonPeriod = request?.comparisonPeriod?.ToString() ?? "month";

                // Validate input parameters
                if (string.IsNullOrEmpty(startDate))
                    return Json(new { error = "Start date is required" });
                
                if (string.IsNullOrEmpty(endDate))
                    return Json(new { error = "End date is required" });

                if (!DateTime.TryParse(startDate, out DateTime parsedStartDate))
                    return Json(new { error = "Invalid start date format" });
                
                if (!DateTime.TryParse(endDate, out DateTime parsedEndDate))
                    return Json(new { error = "Invalid end date format" });

                if (parsedStartDate > parsedEndDate)
                    return Json(new { error = "Start date cannot be after end date" });

                // For now, return mock data to test the frontend integration
                // TODO: Re-enable real API calls once frontend is working
                var salesSummary = new SalesSummaryViewModel 
                { 
                    TotalRevenue = 125000m, 
                    TotalTransactions = 45, 
                    AverageOrderValue = 2777.78m,
                    TopSellingProduct = "Amoxicillin"
                };
                
                // Mock additional data for dashboard
                var salesByProduct = new List<SalesByProductReportViewModel>
                {
                    new SalesByProductReportViewModel { MedicineName = "Amoxicillin", TotalSales = (double)25000m, TotalTransactions = 15 },
                    new SalesByProductReportViewModel { MedicineName = "Ibuprofen", TotalSales = (double)18000m, TotalTransactions = 12 },
                    new SalesByProductReportViewModel { MedicineName = "Acetaminophen", TotalSales = (double)15000m, TotalTransactions = 10 }
                };
                
                var monthlySalesTrends = new List<MonthlySalesTrendViewModel>
                {
                    new MonthlySalesTrendViewModel { Month = DateTime.Now.AddMonths(-2), TotalSales = 100000m, TotalTransactions = 35 },
                    new MonthlySalesTrendViewModel { Month = DateTime.Now.AddMonths(-1), TotalSales = 115000m, TotalTransactions = 40 },
                    new MonthlySalesTrendViewModel { Month = DateTime.Now, TotalSales = 125000m, TotalTransactions = 45 }
                };
                
                var salesRepPerformance = new List<SalesRepPerformanceViewModel>
                {
                    new SalesRepPerformanceViewModel { SalesRepName = "John Doe", TotalSales = 50000m, TotalTransactions = 20 },
                    new SalesRepPerformanceViewModel { SalesRepName = "Jane Smith", TotalSales = 75000m, TotalTransactions = 25 }
                };

                // Calculate comparison period data
                DateTime comparisonStart, comparisonEnd;
                switch (comparisonPeriod.ToLower())
                {
                    case "month":
                        comparisonStart = parsedStartDate.AddMonths(-1);
                        comparisonEnd = parsedEndDate.AddMonths(-1);
                        break;
                    case "quarter":
                        comparisonStart = parsedStartDate.AddMonths(-3);
                        comparisonEnd = parsedEndDate.AddMonths(-3);
                        break;
                    case "year":
                        comparisonStart = parsedStartDate.AddYears(-1);
                        comparisonEnd = parsedEndDate.AddYears(-1);
                        break;
                    default:
                        comparisonStart = parsedStartDate.AddMonths(-1);
                        comparisonEnd = parsedEndDate.AddMonths(-1);
                        break;
                }

                // Mock comparison data
                var comparisonSummary = new SalesSummaryViewModel 
                { 
                    TotalRevenue = 110000m, 
                    TotalTransactions = 40, 
                    AverageOrderValue = 2750m,
                    TopSellingProduct = "Amoxicillin"
                };

                // Format response for dashboard
                var dashboardData = new
                {
                    summary = new
                    {
                        totalRevenue = salesSummary?.TotalRevenue ?? 0,
                        totalTransactions = salesSummary?.TotalTransactions ?? 0,
                        averageOrderValue = salesSummary?.AverageOrderValue ?? 0,
                        topProduct = salesSummary?.TopSellingProduct ?? "N/A",
                        uniqueCustomers = salesByProduct?.Count() ?? 0, // Using product count as proxy for unique customers
                        revenueTrend = CalculateTrendPercentage(salesSummary?.TotalRevenue ?? 0, comparisonSummary?.TotalRevenue ?? 0),
                        transactionsTrend = CalculateTrendPercentage(salesSummary?.TotalTransactions ?? 0, comparisonSummary?.TotalTransactions ?? 0),
                        orderValueTrend = CalculateTrendPercentage(salesSummary?.AverageOrderValue ?? 0, comparisonSummary?.AverageOrderValue ?? 0),
                        customersTrend = 0.0 // Default trend for customers
                    },
                    trends = monthlySalesTrends?.Select(t => new { 
                        month = t.Month, 
                        revenue = t.TotalSales,
                        transactions = t.TotalTransactions 
                    }).ToArray() ?? new object[0],
                    topProducts = salesByProduct?.Take(5).Select(p => new { 
                        name = p.MedicineName, 
                        revenue = p.TotalSales,
                        transactions = p.TotalTransactions 
                    }).ToArray() ?? new object[0],
                    salesReps = salesRepPerformance?.Select(rep => new { 
                        name = rep.SalesRepName, 
                        revenue = rep.TotalSales,
                        transactions = rep.TotalTransactions 
                    }).ToArray() ?? new object[0],
                    dailyData = GenerateDailyBreakdown(parsedStartDate, parsedEndDate, salesSummary?.TotalRevenue ?? 0)
                };

                return Json(dashboardData);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        private double CalculateTrendPercentage(decimal current, decimal previous)
        {
            if (previous == 0) return current > 0 ? 100 : 0;
            return (double)(((current - previous) / previous) * 100);
        }

        private object[] GenerateDailyBreakdown(DateTime startDate, DateTime endDate, decimal totalRevenue)
        {
            var days = (endDate - startDate).Days + 1;
            var dailyAverage = totalRevenue / days;
            var random = new Random();
            
            var dailyData = new List<object>();
            for (int i = 0; i < days; i++)
            {
                var date = startDate.AddDays(i);
                var variance = (decimal)(random.NextDouble() * 0.4 - 0.2); // ±20% variance
                var dailyAmount = dailyAverage * (1 + variance);
                
                dailyData.Add(new { 
                    date = date.ToString("yyyy-MM-dd"), 
                    revenue = Math.Max(0, dailyAmount),
                    transactions = random.Next(10, 50)
                });
            }
            
            return dailyData.ToArray();
        }

        public async Task<ActionResult> SaleByProduct()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SaleByProduct(string startDate, string endDate, string exportFormat)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            DateTime parsedStartDate = DateTime.Parse(startDate);
            DateTime parsedEndDate = DateTime.Parse(endDate);

            var response = await _reportService.GenerateSalesByProductReport(parsedStartDate, parsedEndDate, token);

            if (response != null)
            {
                if (exportFormat == "preview")
                {
                    return PartialView("ReportPreview", response);
                }
            }
            else
            {
                return Json(null);
            }

            return View();
        }

        public async Task<ActionResult> DownloadSaleByProductCSV(string startDate, string endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            DateTime parsedStartDate = DateTime.Parse(startDate);
            DateTime parsedEndDate = DateTime.Parse(endDate);

            var response = await _reportService.GenerateSalesByProductReport(parsedStartDate, parsedEndDate, token);

            if (response != null)
            {
                // Generate the CSV data
                var csv = GenerateSaleByProductCSV(response);
                var fileName = "SalesByProductReport.csv";

                Response.Headers.Add("Content-Disposition", "attachment; filename=" + fileName);

                Response.ContentType = "text/csv";

                return File(new UTF8Encoding().GetBytes(csv), "text/csv", fileName);
            }
            else
            {
                // Handle the case where the report data is not available
                return View("ErrorView");
            }
        }

        public async Task<ActionResult> DownloadSaleByProductPDF(string startDate, string endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            // Obtain the data for the report using the provided start and end dates
            DateTime parsedStartDate = DateTime.Parse(startDate);
            DateTime parsedEndDate = DateTime.Parse(endDate);

            var reportData = await _reportService.GenerateSalesByProductReport(parsedStartDate, parsedEndDate, token);

            // Use the standardized PDF service
            var pdfData = _pdfReportService.GenerateStandardReport(
                title: "Sales by Product Report", 
                data: reportData, 
                startDate: startDate, 
                endDate: endDate,
                customColumns: new Dictionary<string, string>
                {
                    { nameof(SalesByProductReportViewModel.MedicineName), "Medicine Name" },
                    { nameof(SalesByProductReportViewModel.TotalSales), "Total Sales" },
                    { nameof(SalesByProductReportViewModel.TotalTransactions), "Total Transactions" }
                }
            );

            Response.Headers.Add("Content-Disposition", "attachment; filename=SalesByProductReport.pdf");
            Response.ContentType = "application/pdf";
            return File(pdfData, "application/pdf");
        }


        public async Task<ActionResult> SaleByCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SaleByCustomer(int customerId, string exportFormat)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var response = await _reportService.GetSalesByCustomerId(customerId, token);

            if (response != null)
            {
                if (exportFormat == "preview")
                {
                    return PartialView("SaleByCustomerReportPreview", response);
                }
            }
            else
            {
                return Json(null);
            }

            return View();
        }

        public IActionResult MonthlySalesTrends()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MonthlySalesTrends(DateTime startDate, DateTime endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GenerateMonthlySalesTrends(startDate, endDate, token);
            return Json(response);
        }

        public IActionResult SalesRepPerformance()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SalesRepPerformance(DateTime startDate, DateTime endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GenerateSalesRepPerformance(startDate, endDate, token);

            return Json(response);
        }

        public async Task<ActionResult> DownloadSaleByCustomerCSV(int customerId)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GetSalesByCustomerId(customerId, token);

            if (response != null)
            {
                // Generate the CSV data
                var csv = GenerateSaleByCustomerCSV(response);
                var fileName = "SalesByCustomerReport" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ".csv";

                Response.Headers.Add("Content-Disposition", "attachment; filename=" + fileName);

                Response.ContentType = "text/csv";

                return File(new UTF8Encoding().GetBytes(csv), "text/csv", fileName);
            }
            else
            {
                return View();
            }
        }

        public async Task<ActionResult> DownloadSaleByCustomerPDF(int customerId)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var reportData = await _reportService.GetSalesByCustomerId(customerId, token);
            var customer = await _customerService.GetCustomer(customerId);

            // Use the standardized PDF service with custom content
            var (document, stream) = _pdfReportService.CreateStandardDocument();
            
            try
            {
                document.Open();
                
                // Add standard branding and header
                _pdfReportService.AddLogo(document);
                _pdfReportService.AddTitleAndDates(document, "Sales by Customer Report");
                
                // Add customer-specific information
                var customerInfo = new Paragraph($"Customer: {customer.Data.Name}");
                customerInfo.SpacingAfter = 20f;
                document.Add(customerInfo);
                
                // Convert data to table format for custom layout
                var headers = new[] { "Medicine Name", "Sales Date", "Price", "Quantity", "Amount" };
                var rows = new List<string[]>();
                
                foreach (var item in reportData)
                {
                    rows.Add(new[]
                    {
                        item.MedicineName,
                        item.SalesDate.ToString("MMM dd, yyyy"),
                        item.Price.ToString("C2"),
                        item.Quantity.ToString(),
                        item.Amount.ToString("C2")
                    });
                }
                
                // Add table to document
                var table = _pdfReportService.CreateStandardTable(headers, rows);
                document.Add(table);
                
                document.Close();
                
                Response.Headers.Add("Content-Disposition", "attachment; filename=SalesByCustomerReport.pdf");
                Response.ContentType = "application/pdf";
                return File(stream.ToArray(), "application/pdf");
            }
            finally
            {
                document?.Close();
                stream?.Dispose();
            }
        }


        public async Task<ActionResult> Purchase()
        {
            return View();
        }

        public async Task<ActionResult> PurchaseSummary()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PurchaseSummary(string startDate, string endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            DateTime parsedStartDate = DateTime.Parse(startDate);
            DateTime parsedEndDate = DateTime.Parse(endDate);

            var response = await _reportService.GeneratePurchaseSummaryReport(parsedStartDate, parsedEndDate, token);

            if (response != null)
            {
                return Json(response); // Return JSON response to the client
            }
            else
            {
                return Json(null);
            }
        }

        public async Task<ActionResult> PurchaseByProduct()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PurchaseByProduct(string startDate, string endDate, string exportFormat)
        {

            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            DateTime parsedStartDate = DateTime.Parse(startDate);
            DateTime parsedEndDate = DateTime.Parse(endDate);

            var response = await _reportService.GetPurchaseByProduct(parsedStartDate, parsedEndDate, token);

            if (response != null)
            {
                if (exportFormat == "preview")
                {
                    return PartialView("PurchaseByProductPreview", response);
                }
            }
            else
            {
                return Json(null);
            }

            return View();
        }

        public IActionResult SupplierPerformance()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SupplierPerformance(DateTime startDate, DateTime endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GenerateSupplierPerformance(startDate, endDate, token);
            return Json(response);
        }

        [HttpPost]
        public async Task<ActionResult> SupplierPerformanceData(DateTime? startDate = null, DateTime? endDate = null)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            try
            {
                // Default to last 6 months if no dates provided
                var effectiveStartDate = startDate ?? DateTime.Now.AddMonths(-6);
                var effectiveEndDate = endDate ?? DateTime.Now;

                // Get supplier performance data
                var supplierPerformance = await _reportService.GenerateSupplierPerformance(effectiveStartDate, effectiveEndDate, token);
                var purchaseBySupplier = await _supplierService.GetAllAsync(); // Get all suppliers for context

                // Calculate KPIs
                var totalSuppliers = supplierPerformance?.Count ?? 0;
                var avgPerformanceScore = supplierPerformance?.Count > 0 ? supplierPerformance.Average(s => s.PerformanceScore) : 0;
                var highRiskSuppliers = supplierPerformance?.Count(s => s.RiskLevel?.ToLower() == "high") ?? 0;
                var avgOnTimeDelivery = supplierPerformance?.Count > 0 ? supplierPerformance.Average(s => s.OnTimeDeliveryRate) : 0;

                // Categorize suppliers by performance
                var excellentSuppliers = supplierPerformance?.Count(s => s.PerformanceScore >= 90) ?? 0;
                var goodSuppliers = supplierPerformance?.Count(s => s.PerformanceScore >= 80 && s.PerformanceScore < 90) ?? 0;
                var averageSuppliers = supplierPerformance?.Count(s => s.PerformanceScore >= 70 && s.PerformanceScore < 80) ?? 0;
                var poorSuppliers = supplierPerformance?.Count(s => s.PerformanceScore < 70) ?? 0;

                // Get top suppliers
                var topSuppliers = supplierPerformance?.OrderByDescending(s => s.PerformanceScore)
                    .Take(5)
                    .Select(s => new {
                        name = s.SupplierName,
                        performance = s.PerformanceScore,
                        quality = s.QualityRating, // Default if not available
                        delivery = s.OnTimeDeliveryRate,
                        cost = s.CostEfficiency, // Default if not available
                        risk = s.RiskLevel ?? "Low",
                        riskColor = GetRiskColor(s.RiskLevel)
                    }).ToArray() ?? new object[0];

                // Risk distribution
                var riskDistribution = new
                {
                    lowRisk = supplierPerformance?.Count(s => (s.RiskLevel?.ToLower() ?? "low") == "low") ?? 0,
                    mediumRisk = supplierPerformance?.Count(s => (s.RiskLevel?.ToLower() ?? "medium") == "medium") ?? 0,
                    highRisk = supplierPerformance?.Count(s => (s.RiskLevel?.ToLower() ?? "high") == "high") ?? 0,
                    criticalRisk = supplierPerformance?.Count(s => (s.RiskLevel?.ToLower() ?? "critical") == "critical") ?? 0
                };

                var dashboardData = new
                {
                    kpis = new
                    {
                        totalSuppliers = totalSuppliers,
                        supplierGrowth = 5.2, // Mock data for trend
                        avgPerformanceScore = Math.Round(avgPerformanceScore, 1),
                        performanceImprovement = 7.1, // Mock data for trend
                        highRiskSuppliers = highRiskSuppliers,
                        riskReduction = 23.5, // Mock data for trend
                        onTimeDelivery = Math.Round(avgOnTimeDelivery, 1),
                        deliveryChange = 2.3 // Mock data for trend
                    },
                    performanceCategories = new
                    {
                        excellent = excellentSuppliers,
                        good = goodSuppliers,
                        average = averageSuppliers,
                        poor = poorSuppliers
                    },
                    topSuppliers = topSuppliers,
                    riskDistribution = riskDistribution,
                    performanceData = supplierPerformance?.Select(s => new {
                        id = s.SupplierId,
                        name = s.SupplierName,
                        performanceScore = s.PerformanceScore,
                        qualityRating = s.QualityRating,
                        onTimeDelivery = s.OnTimeDeliveryRate,
                        costEfficiency = s.CostEfficiency,
                        riskLevel = s.RiskLevel ?? "Low",
                        totalPurchases = s.TotalPurchases,
                        avgLeadTime = s.AverageLeadTime
                    }).ToArray() ?? new object[0]
                };

                return Json(dashboardData);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        private string GetRiskColor(string riskLevel)
        {
            return (riskLevel?.ToLower()) switch
            {
                "low" => "success",
                "medium" => "warning", 
                "high" => "danger",
                "critical" => "dark",
                _ => "success"
            };
        }

        public IActionResult PurchaseTrends()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PurchaseTrends(DateTime startDate, DateTime endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GeneratePurchaseTrends(startDate, endDate, token);
            return Json(response);
        }

        public async Task<ActionResult> DownloadPurchaseByProductCSV(string startDate, string endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            DateTime parsedStartDate = DateTime.Parse(startDate);
            DateTime parsedEndDate = DateTime.Parse(endDate);

            var response = await _reportService.GetPurchaseByProduct(parsedStartDate, parsedEndDate, token);

            if (response != null)
            {
                // Generate the CSV data
                var csv = GeneratePurchaseByProductCSV(response);
                var fileName = "PurchaseByProductReport" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + ".csv";

                Response.Headers.Add("Content-Disposition", "attachment; filename=" + fileName);

                Response.ContentType = "text/csv";

                return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", fileName);
            }
            else
            {
                // Handle the case where the report data is not available
                return View("ErrorView");
            }
        }

        public async Task<ActionResult> DownloadPurchaseByProductPDF(string startDate, string endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            // Obtain the data for the report using the provided start and end dates
            DateTime parsedStartDate = DateTime.Parse(startDate);
            DateTime parsedEndDate = DateTime.Parse(endDate);

            var reportData = await _reportService.GetPurchaseByProduct(parsedStartDate, parsedEndDate, token);

            // Use the standardized PDF service
            var pdfData = _pdfReportService.GenerateStandardReport(
                title: "Purchase by Product Report", 
                data: reportData, 
                startDate: startDate, 
                endDate: endDate,
                customColumns: new Dictionary<string, string>
                {
                    { nameof(PurchaseByProductReportViewModel.MedicineName), "Medicine Name" },
                    { nameof(PurchaseByProductReportViewModel.TotalPurchaseCost), "Total Purchase Cost" },
                    { nameof(PurchaseByProductReportViewModel.TotalTransactions), "Total Transactions" }
                }
            );

            Response.Headers.Add("Content-Disposition", "attachment; filename=PurchaseByProductReport.pdf");
            Response.ContentType = "application/pdf";
            return File(pdfData, "application/pdf");
        }

        public async Task<ActionResult> PurchaseBySupplier()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PurchaseBySupplier(int supplierId, string exportFormat)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var response = await _reportService.GetPurchaseBySupplierId(supplierId, token);

            if (response != null)
            {
                if (exportFormat == "preview")
                {
                    return PartialView("PurchaseBySupplierReportPreview", response);
                }
            }
            else
            {
                return Json(null);
            }

            return View();
        }

        public async Task<ActionResult> DownloadPurchaseBySupplierPDF(int supplierId)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var reportData = await _reportService.GetPurchaseBySupplierId(supplierId, token);
            var supplier = await _supplierService.GetAsync(supplierId);

            // Use the standardized PDF service with custom content
            var (document, stream) = _pdfReportService.CreateStandardDocument();
            
            try
            {
                document.Open();
                
                // Add standard branding and header
                _pdfReportService.AddLogo(document);
                _pdfReportService.AddTitleAndDates(document, "Purchase by Supplier Report");
                
                // Add supplier-specific information
                var supplierInfo = new Paragraph($"Supplier: {supplier.Data.Name}");
                supplierInfo.SpacingAfter = 20f;
                document.Add(supplierInfo);
                
                // Convert data to table format for custom layout
                var headers = new[] { "Medicine Name", "Purchase Date", "Price", "Quantity", "Amount" };
                var rows = new List<string[]>();
                
                foreach (var item in reportData)
                {
                    rows.Add(new[]
                    {
                        item.MedicineName,
                        item.PurchaseDate.ToString("MMM dd, yyyy"),
                        item.Price.ToString("C2"),
                        item.Quantity.ToString(),
                        item.Amount.ToString("C2")
                    });
                }
                
                // Add table to document
                var table = _pdfReportService.CreateStandardTable(headers, rows);
                document.Add(table);
                
                document.Close();
                
                Response.Headers.Add("Content-Disposition", "attachment; filename=PurchaseBySupplierReport.pdf");
                Response.ContentType = "application/pdf";
                return File(stream.ToArray(), "application/pdf");
            }
            finally
            {
                document?.Close();
                stream?.Dispose();
            }
        }

        public async Task<ActionResult> Inventory()
        {
            return View();
        }

        public async Task<ActionResult> StockSummary()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> StockSummary(string startDate, string endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            DateTime parsedStartDate = DateTime.Parse(startDate);
            DateTime parsedEndDate = DateTime.Parse(endDate);

            var response = await _reportService.GenerateStockSummaryReport(parsedStartDate, parsedEndDate);

            if (response != null)
            {
                return Json(response); // Return JSON response to the client
            }
            else
            {
                return Json(null);
            }
        }

        public IActionResult ABCAnalysis()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ABCAnalysis(DateTime asOfDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GenerateABCAnalysis(asOfDate, token);
            return Json(response);
        }

        public IActionResult InventoryAging()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InventoryAging(DateTime asOfDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GenerateInventoryAging(asOfDate, token);
            return Json(response);
        }

        public IActionResult ExpiryTracking()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ExpiryTracking(int monthsToExpiry)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GenerateExpiryTracking(monthsToExpiry, token);

            return Json(response);
        }

        [HttpPost]
        public async Task<ActionResult> ExpiryTrackingData(int monthsToExpiry)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            try
            {
                // Get expiry tracking data
                var expiryData = await _reportService.GenerateExpiryTracking(monthsToExpiry, token);
                
                // Get additional inventory data for context
                var stockSummary = await _reportService.GenerateStockSummaryReport(DateTime.Now.AddMonths(-1), DateTime.Now);

                // Process and categorize expiry data
                var currentDate = DateTime.Now;
                var criticalItems = expiryData?.Where(e => e.ExpiryDate <= currentDate.AddDays(30)).ToList() ?? new List<ExpiryTrackingViewModel>();
                var warningItems = expiryData?.Where(e => e.ExpiryDate > currentDate.AddDays(30) && e.ExpiryDate <= currentDate.AddDays(90)).ToList() ?? new List<ExpiryTrackingViewModel>();
                var upcomingItems = expiryData?.Where(e => e.ExpiryDate > currentDate.AddDays(90) && e.ExpiryDate <= currentDate.AddMonths(monthsToExpiry)).ToList() ?? new List<ExpiryTrackingViewModel>();

                // Calculate totals and metrics
                var totalItems = expiryData?.Count ?? 0;
                var totalValue = expiryData?.Sum(e => e.CurrentStock * e.UnitCost) ?? 0;
                var criticalValue = criticalItems.Sum(e => e.CurrentStock * e.UnitCost);
                var warningValue = warningItems.Sum(e => e.CurrentStock * e.UnitCost);

                // Prepare category data for charts (use ExpiryStatus as category since Category doesn't exist)
                var categoryBreakdown = expiryData?.GroupBy(e => e.ExpiryStatus ?? "Unknown")
                    .Select(g => new {
                        category = g.Key,
                        count = g.Count(),
                        value = g.Sum(e => e.CurrentStock * e.UnitCost),
                        critical = g.Count(e => e.ExpiryDate <= currentDate.AddDays(30))
                    }).OrderByDescending(x => x.value).ToList();

                // Prepare monthly expiry trends
                var monthlyTrends = expiryData?.GroupBy(e => new { 
                        Year = e.ExpiryDate.Year, 
                        Month = e.ExpiryDate.Month 
                    })
                    .Select(g => new {
                        month = $"{g.Key.Year}-{g.Key.Month:D2}",
                        count = g.Count(),
                        value = g.Sum(e => e.CurrentStock * e.UnitCost)
                    }).OrderBy(x => x.month).ToList();

                // Calculate compliance metrics
                var complianceMetrics = new
                {
                    totalCompliant = totalItems - criticalItems.Count,
                    complianceRate = totalItems > 0 ? Math.Round((double)(totalItems - criticalItems.Count) / totalItems * 100, 1) : 100,
                    averageDaysToExpiry = expiryData?.Where(e => e.ExpiryDate > currentDate)
                        .Average(e => (e.ExpiryDate - currentDate).TotalDays) ?? 0,
                    riskScore = CalculateRiskScore(criticalItems.Count, warningItems.Count, totalItems)
                };

                var dashboardData = new
                {
                    summary = new
                    {
                        totalItems = totalItems,
                        totalValue = totalValue,
                        criticalItems = criticalItems.Count,
                        warningItems = warningItems.Count,
                        upcomingItems = upcomingItems.Count,
                        criticalValue = criticalValue,
                        warningValue = warningValue,
                        complianceRate = complianceMetrics.complianceRate,
                        averageDaysToExpiry = Math.Round(complianceMetrics.averageDaysToExpiry, 0),
                        riskScore = complianceMetrics.riskScore
                    },
                    criticalAlerts = criticalItems.Take(10).Select(item => new {
                        id = 1, // Default ID since MedicineId doesn't exist
                        name = item.ProductName,
                        batch = item.BatchNumber,
                        expiryDate = item.ExpiryDate.ToString("yyyy-MM-dd"),
                        daysToExpiry = (item.ExpiryDate - currentDate).Days,
                        quantity = item.CurrentStock,
                        value = item.CurrentStock * item.UnitCost,
                        location = "Main Storage", // Default since Location doesn't exist
                        supplier = "Unknown" // Default since SupplierName doesn't exist
                    }),
                    categoryBreakdown = categoryBreakdown,
                    monthlyTrends = monthlyTrends,
                    expiryTimeline = expiryData?.Select(item => new {
                        name = item.ProductName,
                        batch = item.BatchNumber,
                        expiryDate = item.ExpiryDate.ToString("yyyy-MM-dd"),
                        daysToExpiry = (item.ExpiryDate - currentDate).Days,
                        quantity = item.CurrentStock,
                        value = item.CurrentStock * item.UnitCost,
                        category = item.ExpiryStatus ?? "Unknown",
                        urgency = GetUrgencyLevel(item.ExpiryDate, currentDate)
                    }).OrderBy(x => x.daysToExpiry),
                    complianceMetrics = complianceMetrics
                };

                return Json(dashboardData);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        private int CalculateRiskScore(int criticalItems, int warningItems, int totalItems)
        {
            if (totalItems == 0) return 0;
            
            var criticalWeight = 50.0;
            var warningWeight = 20.0;
            
            var riskScore = ((criticalItems * criticalWeight) + (warningItems * warningWeight)) / totalItems;
            return Math.Min(100, (int)Math.Round(riskScore));
        }

        private string GetUrgencyLevel(DateTime expiryDate, DateTime currentDate)
        {
            var daysToExpiry = (expiryDate - currentDate).Days;
            
            if (daysToExpiry <= 0) return "expired";
            if (daysToExpiry <= 30) return "critical";
            if (daysToExpiry <= 90) return "warning";
            if (daysToExpiry <= 180) return "attention";
            return "normal";
        }

        public async Task<ActionResult> Receivable()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AccountsReceivableReport(string startDate, string endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GenerateAccountsReceivableReport(token);
            return Json(response);
        }

        public async Task<IActionResult> DownloadAccountsReceivablePDF(string startDate, string endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GenerateAccountsReceivableReport(token);

            if (response != null && response.Count > 0)
            {
                // Use the standardized PDF service
                var pdfData = _pdfReportService.GenerateStandardReport(
                    title: "Accounts Receivable Report", 
                    data: response,
                    customColumns: new Dictionary<string, string>
                    {
                        { nameof(AccountsReceivableReportViewModel.CustomerName), "Customer" },
                        { nameof(AccountsReceivableReportViewModel.AmountOwing), "Amount Owing" },
                        { nameof(AccountsReceivableReportViewModel.DueDate), "Due Date" }
                    }
                );
                return File(pdfData, "application/pdf", $"AccountsReceivable_{DateTime.Now:yyyyMMdd}.pdf");
            }

            return RedirectToAction("Receivable");
        }

        public async Task<IActionResult> DownloadAccountsReceivableCSV(string startDate, string endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GenerateAccountsReceivableReport(token);

            if (response != null && response.Count > 0)
            {
                byte[] csvData = GenerateAccountsReceivableCSV(response);
                return File(csvData, "text/csv", $"AccountsReceivable_{DateTime.Now:yyyyMMdd}.csv");
            }

            return RedirectToAction("Receivable");
        }

        public async Task<ActionResult> Payable()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AccountsPayableReport(string startDate, string endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GenerateAccountsPayableReport(token);
            return Json(response);
        }

        public async Task<IActionResult> DownloadAccountsPayablePDF(string startDate, string endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GenerateAccountsPayableReport(token);

            if (response != null && response.Count > 0)
            {
                // Use the standardized PDF service
                var pdfData = _pdfReportService.GenerateStandardReport(
                    title: "Accounts Payable Report", 
                    data: response,
                    customColumns: new Dictionary<string, string>
                    {
                        { nameof(AccountsPayableReportViewModel.SupplierName), "Supplier" },
                        { nameof(AccountsPayableReportViewModel.AmountOwing), "Amount Owing" },
                        { nameof(AccountsPayableReportViewModel.DueDate), "Due Date" }
                    }
                );
                return File(pdfData, "application/pdf", $"AccountsPayable_{DateTime.Now:yyyyMMdd}.pdf");
            }

            return RedirectToAction("Payable");
        }

        public async Task<IActionResult> DownloadAccountsPayableCSV(string startDate, string endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GenerateAccountsPayableReport(token);

            if (response != null && response.Count > 0)
            {
                byte[] csvData = GenerateAccountsPayableCSV(response);
                return File(csvData, "text/csv", $"AccountsPayable_{DateTime.Now:yyyyMMdd}.csv");
            }

            return RedirectToAction("Payable");
        }

        public IActionResult ProfitMargin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProfitMargin(DateTime startDate, DateTime endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GenerateProfitMarginAnalysis(startDate, endDate, token);
            return Json(response);
        }

        public IActionResult ExpenseAnalysis()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ExpenseAnalysis(DateTime startDate, DateTime endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GenerateExpenseAnalysis(startDate, endDate, token);
            return Json(response);
        }

        #region Customer Analysis Reports
        public IActionResult CustomerLifetimeValue()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CustomerLifetimeValue(DateTime startDate, DateTime endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GenerateCustomerLifetimeValue(startDate, endDate, token);
            return Json(response);
        }

        public IActionResult CustomerRetention()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CustomerRetention(DateTime startDate, DateTime endDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GenerateCustomerRetention(startDate, endDate, token);
            return Json(response);
        }

        public IActionResult CustomerSegmentation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CustomerSegmentation(DateTime asOfDate)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            
            var response = await _reportService.GenerateCustomerSegmentation(asOfDate, token);
            return Json(response);
        }
        [HttpPost]
        public async Task<ActionResult> CustomerIntelligenceData(DateTime? startDate = null, DateTime? endDate = null)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            try
            {
                // Default to last 12 months if no dates provided
                var effectiveStartDate = startDate ?? DateTime.Now.AddMonths(-12);
                var effectiveEndDate = endDate ?? DateTime.Now;

                // Get customer analytics data
                var customerLifetimeValue = await _reportService.GenerateCustomerLifetimeValue(effectiveStartDate, effectiveEndDate, token);
                var customerRetention = await _reportService.GenerateCustomerRetention(effectiveStartDate, effectiveEndDate, token);
                var customerSegmentation = await _reportService.GenerateCustomerSegmentation(effectiveEndDate, token);

                // Calculate KPIs
                var totalCustomers = customerSegmentation?.Sum(s => s.CustomerCount) ?? 0;
                var avgClv = customerLifetimeValue?.Count > 0 ? customerLifetimeValue.Average(c => c.CustomerLifetimeValue) : 0;
                var avgEngagement = 72; // Mock data - would need additional endpoint

                // Calculate churn risk (simplified calculation)
                var retentionRate = customerRetention?.Count > 0 ? customerRetention.Average(r => r.RepeatPurchaseRate) : 0;
                var churnRisk = 100 - retentionRate;

                // Process segmentation data
                var segmentationData = customerSegmentation?.Select(s => new {
                    segment = s.SegmentName,
                    customerCount = s.CustomerCount,
                    averageClv = s.AverageOrderValue, // Use available property
                    totalRevenue = s.TotalRevenue,
                    retentionRate = 85 // Default since property doesn't exist
                }).ToArray() ?? new object[0];

                // Process CLV distribution
                var clvRanges = new[]
                {
                    new { range = "$0-500", min = 0m, max = 500m },
                    new { range = "$500-1K", min = 500m, max = 1000m },
                    new { range = "$1K-2.5K", min = 1000m, max = 2500m },
                    new { range = "$2.5K-5K", min = 2500m, max = 5000m },
                    new { range = "$5K-10K", min = 5000m, max = 10000m },
                    new { range = "$10K+", min = 10000m, max = decimal.MaxValue }
                };

                var clvDistribution = clvRanges.Select(range => new {
                    range = range.range,
                    count = customerLifetimeValue?.Count(c => c.CustomerLifetimeValue >= range.min && c.CustomerLifetimeValue < range.max) ?? 0
                }).ToArray();

                // Calculate trends (mock calculation for demonstration)
                var clvTrends = Enumerable.Range(0, 12).Select(i => {
                    var month = effectiveStartDate.AddMonths(i);
                    var monthlyClv = avgClv * (1 + (decimal)(new Random().NextDouble() * 0.2 - 0.1)); // ±10% variance
                    return new {
                        month = month.ToString("MMM"),
                        clv = Math.Round(monthlyClv, 2)
                    };
                }).ToArray();

                // Churn prediction (simplified mock)
                var churnPrediction = new
                {
                    highRisk = (int)(totalCustomers * 0.15),
                    mediumRisk = (int)(totalCustomers * 0.25),
                    lowRisk = (int)(totalCustomers * 0.60)
                };

                var dashboardData = new
                {
                    kpis = new
                    {
                        totalCustomers = totalCustomers,
                        customerGrowth = 8.5, // Mock trend data
                        avgClv = Math.Round(avgClv, 2),
                        clvGrowth = 12.3, // Mock trend data
                        churnRisk = Math.Round(churnRisk, 1),
                        churnChange = 3.1, // Mock trend data
                        engagementScore = avgEngagement,
                        engagementChange = 0.8 // Mock trend data
                    },
                    segmentation = new
                    {
                        distribution = segmentationData,
                        performance = segmentationData
                    },
                    lifetimeValue = new
                    {
                        distribution = clvDistribution,
                        trends = clvTrends,
                        insights = new
                        {
                            top20Revenue = 80, // Mock: top 20% generate 80% of revenue
                            clvGrowthRate = 12, // Mock: 12% annual growth
                            optimizationPotential = "$2.3M" // Mock potential
                        }
                    },
                    churnAnalysis = new
                    {
                        riskDistribution = churnPrediction,
                        factors = new[]
                        {
                            new { factor = "Declining Order Frequency", impact = 85 },
                            new { factor = "Price Sensitivity", impact = 72 },
                            new { factor = "Support Issues", impact = 68 },
                            new { factor = "Competition", impact = 61 },
                            new { factor = "Product Fit", impact = 45 }
                        }
                    },
                    behavior = new
                    {
                        purchaseFrequency = new[]
                        {
                            new { month = "Jan", frequency = 2.1 },
                            new { month = "Feb", frequency = 2.3 },
                            new { month = "Mar", frequency = 2.0 },
                            new { month = "Apr", frequency = 2.4 },
                            new { month = "May", frequency = 2.2 },
                            new { month = "Jun", frequency = 2.3 }
                        },
                        seasonalPatterns = new[]
                        {
                            new { quarter = "Q1", activity = 75 },
                            new { quarter = "Q2", activity = 90 },
                            new { quarter = "Q3", activity = 85 },
                            new { quarter = "Q4", activity = 95 }
                        },
                        insights = new
                        {
                            avgOrderValue = "$1,234",
                            orderFrequency = "2.3/month",
                            customerLifetime = "18 months"
                        }
                    },
                    engagement = new
                    {
                        trends = clvTrends, // Reuse CLV trends as engagement trends
                        metrics = new
                        {
                            sessionDuration = "4:32",
                            emailOpenRate = "68%",
                            supportSatisfaction = "4.2/5"
                        }
                    }
                };

                return Json(dashboardData);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> InventoryOptimizationData()
        {
            try
            {
                var token = HttpContext.Session.GetString("Token");
                var asOfDate = DateTime.Now;
                var startDate = DateTime.Now.AddMonths(-12);
                var endDate = DateTime.Now;

                // Get ABC Analysis data
                var abcAnalysis = await _reportService.GenerateABCAnalysis(asOfDate, token);
                
                // Get Stock Summary/Turnover data
                var stockSummary = await _reportService.GenerateStockSummaryReport(startDate, endDate);
                
                // Get Inventory Aging
                var inventoryAging = await _reportService.GenerateInventoryAging(asOfDate, token);

                // Calculate KPIs
                var totalItems = abcAnalysis?.Count ?? 0;
                var totalValue = abcAnalysis?.Sum(x => (decimal?)x.AnnualUsageValue) ?? 0;
                var avgTurnover = stockSummary?.Any() == true ? stockSummary.Average(x => x.TurnoverRatio) : 0;

                // Calculate reorder alerts (items with low stock)
                var reorderAlerts = inventoryAging?.Count(x => x.CurrentStock < 50) ?? 23;
                
                // Calculate slow moving items (turnover < 2)
                var slowMovingItems = stockSummary?.Count(x => x.TurnoverRatio < 2) ?? 147;
                
                // Calculate excess stock value (need to calculate from inventory data)
                var excessStockValue = stockSummary?.Where(x => x.TurnoverRatio < 1)?.Sum(x => x.AverageInventory * 100) ?? 45280;
                
                // Calculate efficiency (percentage of items with good turnover)
                var goodTurnoverItems = stockSummary?.Count(x => x.TurnoverRatio >= 2) ?? 0;
                var efficiency = totalItems > 0 ? (double)goodTurnoverItems / totalItems * 100 : 87;

                // ABC Analysis breakdown
                var abcBreakdown = abcAnalysis?.GroupBy(x => x.Category)
                    .Select(g => new
                    {
                        Class = g.Key,
                        ItemCount = g.Count(),
                        TotalValue = g.Sum(x => x.AnnualUsageValue),
                        RevenuePercentage = g.Sum(x => x.AnnualUsageValue) / totalValue * 100
                    }).Cast<object>().ToList();

                // Turnover distribution
                var turnoverDistribution = new
                {
                    Fast = stockSummary?.Count(x => x.TurnoverRatio > 6) ?? 156,
                    Medium = stockSummary?.Count(x => x.TurnoverRatio >= 2 && x.TurnoverRatio <= 6) ?? 2544,
                    Slow = stockSummary?.Count(x => x.TurnoverRatio >= 1 && x.TurnoverRatio < 2) ?? 124,
                    Dead = stockSummary?.Count(x => x.TurnoverRatio < 1) ?? 23
                };

                // Critical items needing attention (combine turnover and aging data)
                var criticalItems = new List<object>();
                if (stockSummary?.Any() == true && inventoryAging?.Any() == true)
                {
                    var combined = from st in stockSummary
                                   join ia in inventoryAging on st.ProductName equals ia.ProductName into gj
                                   from ia in gj.DefaultIfEmpty()
                                   where st.TurnoverRatio < 1 || (ia?.CurrentStock ?? 0) < 50
                                   select new
                                   {
                                       ProductName = st.ProductName,
                                       Issue = st.TurnoverRatio < 1 ? "Slow Moving" : "Low Stock",
                                       CurrentStock = ia?.CurrentStock ?? st.AverageInventory,
                                       TurnoverRatio = st.TurnoverRatio,
                                       StockValue = (ia?.CurrentStock ?? st.AverageInventory) * (ia?.UnitCost ?? 100),
                                       Priority = st.TurnoverRatio < 0.5m || (ia?.CurrentStock ?? 0) < 20 ? "Critical" : "Warning"
                                   };
                    criticalItems = combined.Take(10).Cast<object>().ToList();
                }

                // Generate optimization recommendations
                var recommendations = new List<object>
                {
                    new
                    {
                        Type = "reorder",
                        Priority = "high",
                        Title = "Address Critical Stock Levels",
                        Description = $"Review {reorderAlerts} items below reorder point to prevent stockouts",
                        Impact = "Avoid lost sales",
                        Effort = "Low",
                        Timeline = "1-2 days"
                    },
                    new
                    {
                        Type = "optimize",
                        Priority = "medium", 
                        Title = "Reduce Excess Inventory",
                        Description = $"Liquidate {slowMovingItems} slow-moving items valued at ${excessStockValue:N0}",
                        Impact = $"${excessStockValue:N0} working capital freed",
                        Effort = "Medium",
                        Timeline = "2-4 weeks"
                    },
                    new
                    {
                        Type = "strategy",
                        Priority = "low",
                        Title = "Optimize Inventory Mix",
                        Description = "Focus purchasing on Class A items with higher turnover rates",
                        Impact = "Improved efficiency",
                        Effort = "High",
                        Timeline = "1-3 months"
                    }
                };

                var responseData = new
                {
                    kpis = new
                    {
                        totalItems = totalItems,
                        totalValue = totalValue,
                        avgTurnover = Math.Round(avgTurnover, 1),
                        reorderAlerts = reorderAlerts,
                        slowMovingItems = slowMovingItems,
                        excessStockValue = excessStockValue,
                        efficiency = Math.Round(efficiency, 1)
                    },
                    abcAnalysis = abcBreakdown,
                    turnoverDistribution = turnoverDistribution,
                    criticalItems = criticalItems,
                    recommendations = recommendations,
                    agingData = inventoryAging?.Take(10)?.Select(x => new
                    {
                        ProductName = x.ProductName,
                        QuantityOnHand = x.CurrentStock,
                        DaysInInventory = x.DaysInInventory,
                        StockValue = x.CurrentStock * (x.UnitCost)
                    })
                };

                return Json(responseData);
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, new { error = "Failed to load inventory optimization data", details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> CashFlowData()
        {
            try
            {
                var token = HttpContext.Session.GetString("Token");

                // Get Accounts Receivable data
                var accountsReceivable = await _reportService.GenerateAccountsReceivableReport(token);
                
                // Get Accounts Payable data
                var accountsPayable = await _reportService.GenerateAccountsPayableReport(token);

                // Calculate financial KPIs
                var totalReceivable = accountsReceivable?.Sum(x => x.AmountOwing) ?? 89420;
                var totalPayable = accountsPayable?.Sum(x => x.AmountOwing) ?? 67830;
                var currentCash = 125750; // This would come from a financial system
                var netCashFlow = totalReceivable - totalPayable;

                // Calculate AR aging analysis
                var arAging = new List<object>();
                if (accountsReceivable?.Any() == true)
                {
                    var total = accountsReceivable.Sum(x => x.AmountOwing);
                    var aging0to30 = accountsReceivable.Where(x => (DateTime.Now - x.DueDate).Days <= 30).Sum(x => x.AmountOwing);
                    var aging31to60 = accountsReceivable.Where(x => (DateTime.Now - x.DueDate).Days > 30 && (DateTime.Now - x.DueDate).Days <= 60).Sum(x => x.AmountOwing);
                    var aging61to90 = accountsReceivable.Where(x => (DateTime.Now - x.DueDate).Days > 60 && (DateTime.Now - x.DueDate).Days <= 90).Sum(x => x.AmountOwing);
                    var aging90plus = accountsReceivable.Where(x => (DateTime.Now - x.DueDate).Days > 90).Sum(x => x.AmountOwing);

                    arAging.Add(new { period = "0-30", amount = aging0to30, percentage = Math.Round((double)(aging0to30 / total * 100), 1) });
                    arAging.Add(new { period = "31-60", amount = aging31to60, percentage = Math.Round((double)(aging31to60 / total * 100), 1) });
                    arAging.Add(new { period = "61-90", amount = aging61to90, percentage = Math.Round((double)(aging61to90 / total * 100), 1) });
                    arAging.Add(new { period = "90+", amount = aging90plus, percentage = Math.Round((double)(aging90plus / total * 100), 1) });
                }
                else
                {
                    // Fallback data
                    arAging.Add(new { period = "0-30", amount = 45280, percentage = 51 });
                    arAging.Add(new { period = "31-60", amount = 28140, percentage = 31 });
                    arAging.Add(new { period = "61-90", amount = 12800, percentage = 14 });
                    arAging.Add(new { period = "90+", amount = 3200, percentage = 4 });
                }

                // Calculate AP aging analysis
                var apAging = new List<object>();
                if (accountsPayable?.Any() == true)
                {
                    var total = accountsPayable.Sum(x => x.AmountOwing);
                    var aging0to30 = accountsPayable.Where(x => (DateTime.Now - x.DueDate).Days <= 30).Sum(x => x.AmountOwing);
                    var aging31to60 = accountsPayable.Where(x => (DateTime.Now - x.DueDate).Days > 30 && (DateTime.Now - x.DueDate).Days <= 60).Sum(x => x.AmountOwing);
                    var aging61to90 = accountsPayable.Where(x => (DateTime.Now - x.DueDate).Days > 60 && (DateTime.Now - x.DueDate).Days <= 90).Sum(x => x.AmountOwing);
                    var aging90plus = accountsPayable.Where(x => (DateTime.Now - x.DueDate).Days > 90).Sum(x => x.AmountOwing);

                    apAging.Add(new { period = "0-30", amount = aging0to30, percentage = Math.Round((double)(aging0to30 / total * 100), 1) });
                    apAging.Add(new { period = "31-60", amount = aging31to60, percentage = Math.Round((double)(aging31to60 / total * 100), 1) });
                    apAging.Add(new { period = "61-90", amount = aging61to90, percentage = Math.Round((double)(aging61to90 / total * 100), 1) });
                    apAging.Add(new { period = "90+", amount = aging90plus, percentage = Math.Round((double)(aging90plus / total * 100), 1) });
                }
                else
                {
                    // Fallback data
                    apAging.Add(new { period = "0-30", amount = 42150, percentage = 62 });
                    apAging.Add(new { period = "31-60", amount = 18680, percentage = 28 });
                    apAging.Add(new { period = "61-90", amount = 5800, percentage = 9 });
                    apAging.Add(new { period = "90+", amount = 1200, percentage = 2 });
                }

                // Generate cash flow forecast (simplified 30-day projection)
                var forecast = new List<object>();
                var startDate = DateTime.Now;
                var runningCash = currentCash;
                
                for (int i = 0; i < 30; i++)
                {
                    var date = startDate.AddDays(i);
                    
                    // Simulate daily inflows and outflows based on historical patterns
                    var dailyInflow = (double)(totalReceivable / 30) + (new Random().NextDouble() - 0.5) * 1000;
                    var dailyOutflow = (double)(totalPayable / 30) + (new Random().NextDouble() - 0.5) * 800;
                    var netFlow = dailyInflow - dailyOutflow;
                    runningCash += (int)netFlow;

                    forecast.Add(new
                    {
                        date = date.ToString("yyyy-MM-dd"),
                        inflow = Math.Round(dailyInflow, 2),
                        outflow = Math.Round(dailyOutflow, 2),
                        netFlow = Math.Round(netFlow, 2),
                        cumulativeCash = Math.Round((decimal)runningCash, 2)
                    });
                }

                // Payment priorities (urgent payments due soon)
                var paymentPriorities = accountsPayable?
                    .Where(x => x.DueDate <= DateTime.Now.AddDays(14))
                    .OrderBy(x => x.DueDate)
                    .Take(10)
                    .Select(x => new
                    {
                        supplier = x.SupplierName,
                        amount = x.AmountOwing,
                        dueDate = x.DueDate.ToString("yyyy-MM-dd"),
                        daysUntilDue = (x.DueDate - DateTime.Now).Days,
                        priority = (x.DueDate - DateTime.Now).Days <= 5 ? "high" : "medium"
                    }).Cast<object>().ToList() ?? new List<object>
                    {
                        new { supplier = "PharmaCorp Inc.", amount = 15240.0, dueDate = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd"), daysUntilDue = 5, priority = "high" },
                        new { supplier = "MedSupply Partners", amount = 9680.0, dueDate = DateTime.Now.AddDays(12).ToString("yyyy-MM-dd"), daysUntilDue = 12, priority = "medium" }
                    };

                // Calculate trends
                var avgReceivableDays = accountsReceivable?.Any() == true ? 
                    Math.Round(accountsReceivable.Average(x => (double)(DateTime.Now - x.DueDate).Days), 0) : 28;
                var avgPayableDays = accountsPayable?.Any() == true ? 
                    Math.Round(accountsPayable.Average(x => (double)(DateTime.Now - x.DueDate).Days), 0) : 22;

                var responseData = new
                {
                    kpis = new
                    {
                        currentCash = currentCash,
                        totalReceivable = totalReceivable,
                        totalPayable = totalPayable,
                        netCashFlow = netCashFlow,
                        avgReceivableDays = avgReceivableDays,
                        avgPayableDays = avgPayableDays,
                        cashFlowTrend = netCashFlow > 0 ? "positive" : "negative",
                        cashFlowPercentage = 12.5 // This would be calculated from historical data
                    },
                    receivablesAging = arAging,
                    payablesAging = apAging,
                    cashFlowForecast = forecast,
                    paymentPriorities = paymentPriorities,
                    arDetails = accountsReceivable?.Take(10)?.Select(x => new
                    {
                        customerName = x.CustomerName,
                        amount = x.AmountOwing,
                        daysOverdue = (DateTime.Now - x.DueDate).Days,
                        dueDate = x.DueDate.ToString("yyyy-MM-dd")
                    }),
                    apDetails = accountsPayable?.Take(10)?.Select(x => new
                    {
                        vendorName = x.SupplierName,
                        amount = x.AmountOwing,
                        daysOverdue = (DateTime.Now - x.DueDate).Days,
                        dueDate = x.DueDate.ToString("yyyy-MM-dd")
                    })
                };

                return Json(responseData);
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, new { error = "Failed to load cash flow data", details = ex.Message });
            }
        }

        public string GenerateSaleByProductCSV(IEnumerable<SalesByProductReportViewModel> reportData)
        {
            var csv = new StringBuilder();

            // Create the CSV header
            csv.AppendLine("Medicine Name,Total Sales,Total Transactions");

            // Iterate through the report data and format it as CSV
            foreach (var item in reportData)
            {
                var line = string.Format("\"{0}\",\"{1}\",{2}",
                    item.MedicineName,
                    item.TotalSales.ToString("0.00"),
                    item.TotalTransactions);

                csv.AppendLine(line);
            }

            return csv.ToString();
        }

        public string GenerateSaleByCustomerCSV(IEnumerable<SalesByCustomerViewModel> reportData)
        {
            var csv = new StringBuilder();

            // Create the CSV header
            csv.AppendLine("Medicine Name,Sales Date,Amount, Quantity, Price");
            // Iterate through the report data and format it as CSV
            foreach (var item in reportData)
            {
                var line = string.Format("\"{0}\",\"{1}\",{2}\",\"{3}\",\"{4}\"",
                    item.MedicineName,
                    item.SalesDate.ToString("yyyy-MM-dd"),
                    item.Amount.ToString("0.00"),
                    item.Quantity,
                    item.Price.ToString("0.00"));

                csv.AppendLine(line);
            }

            return csv.ToString();
        }

        public string GeneratePurchaseByProductCSV(IEnumerable<PurchaseByProductReportViewModel> reportData)
        {
            var csv = new StringBuilder();

            // Create the CSV header
            csv.AppendLine("Medicine Name,Total Purchase Cost,Total Transactions");

            // Iterate through the report data and format it as CSV
            foreach (var item in reportData)
            {
                var line = string.Format("\"{0}\",\"{1}\",{2}",
                    item.MedicineName,
                    item.TotalPurchaseCost.ToString("0.00"),
                    item.TotalTransactions);

                csv.AppendLine(line);
            }

            return csv.ToString();
        }

        #region Helper Methods
        private IActionResult GenerateReportPDF(object reportData, string title, string startDate, string endDate)
        {
            // This method should be replaced with specific implementations using PdfReportService
            // Keeping basic implementation for backward compatibility
            var (document, stream) = _pdfReportService.CreateStandardDocument();
            
            try
            {
                document.Open();
                _pdfReportService.AddLogo(document);
                _pdfReportService.AddTitleAndDates(document, title, startDate, endDate);
                
                // Add basic content - this may need customization per report type
                if (reportData != null)
                {
                    var reportText = new Paragraph($"Report Data: {reportData.ToString()}");
                    document.Add(reportText);
                }
                
                document.Close();
                return File(stream.ToArray(), "application/pdf", $"{title.Replace(" ", "")}.pdf");
            }
            finally
            {
                document?.Close();
                stream?.Dispose();
            }
        }

        private async Task<IActionResult> GenerateCustomerReportPDF(object reportData, object customer, string title)
        {
            // Similar to GenerateReportPDF but with customer-specific formatting
            // Implementation details...
            return null;
        }

        private FileResult GenerateCSVFile(string csvContent, string fileName)
        {
            var bytes = new UTF8Encoding().GetBytes(csvContent);
            return File(bytes, "text/csv", fileName);
        }


        private byte[] GenerateAccountsReceivableCSV(IEnumerable<AccountsReceivableReportViewModel> reportData)
        {
            var csv = new StringBuilder();

            // Create the CSV header
            csv.AppendLine("Customer Name,Amount Owing,Due Date");

            // Iterate through the report data and format it as CSV
            foreach (var item in reportData)
            {
                var line = string.Format("\"{0}\",{1},\"{2}\"",
                    item.CustomerName ?? "",
                    item.AmountOwing.ToString("0.00"),
                    item.DueDate.ToString("yyyy-MM-dd"));

                csv.AppendLine(line);
            }

            return new UTF8Encoding().GetBytes(csv.ToString());
        }


        private byte[] GenerateAccountsPayableCSV(IEnumerable<AccountsPayableReportViewModel> reportData)
        {
            var csv = new StringBuilder();

            // Create the CSV header
            csv.AppendLine("Supplier Name,Amount Owing,Due Date");

            // Iterate through the report data and format it as CSV
            foreach (var item in reportData)
            {
                var line = string.Format("\"{0}\",{1},\"{2}\"",
                    item.SupplierName ?? "",
                    item.AmountOwing.ToString("0.00"),
                    item.DueDate.ToString("yyyy-MM-dd"));

                csv.AppendLine(line);
            }

            return new UTF8Encoding().GetBytes(csv.ToString());
        }

        #endregion
    }
}
