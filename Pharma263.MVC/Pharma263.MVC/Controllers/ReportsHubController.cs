using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Pharma263.MVC.Controllers
{
    /// <summary>
    /// Reports Hub - Central directory for all 28+ reports
    /// Phase 6: Reporting & UX Enhancement
    /// </summary>
    [Authorize]
    public class ReportsHubController : Controller
    {
        private const string RECENTLY_VIEWED_KEY = "RecentlyViewedReports";
        private const int MAX_RECENT_REPORTS = 5;

        public IActionResult Index(string category = null, string search = null)
        {
            var allReports = GetAllReports();

            // Filter by category if specified
            if (!string.IsNullOrEmpty(category))
            {
                allReports = allReports.Where(r => r.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filter by search term if specified
            if (!string.IsNullOrEmpty(search))
            {
                allReports = allReports.Where(r =>
                    r.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    r.Description.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    r.Category.Contains(search, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            // Get recently viewed reports
            var recentlyViewed = GetRecentlyViewedReports();

            ViewBag.Categories = GetCategories();
            ViewBag.RecentlyViewed = recentlyViewed;
            ViewBag.CurrentCategory = category;
            ViewBag.SearchTerm = search;

            return View(allReports);
        }

        [HttpPost]
        public IActionResult TrackReportView(string reportTitle, string reportUrl)
        {
            if (string.IsNullOrEmpty(reportTitle) || string.IsNullOrEmpty(reportUrl))
            {
                return BadRequest();
            }

            var recentlyViewed = GetRecentlyViewedReports();

            // Remove if already exists (to move it to top)
            recentlyViewed.RemoveAll(r => r.Url.Equals(reportUrl, StringComparison.OrdinalIgnoreCase));

            // Add to top
            recentlyViewed.Insert(0, new RecentReport
            {
                Title = reportTitle,
                Url = reportUrl,
                ViewedAt = DateTime.Now
            });

            // Keep only MAX_RECENT_REPORTS
            if (recentlyViewed.Count > MAX_RECENT_REPORTS)
            {
                recentlyViewed = recentlyViewed.Take(MAX_RECENT_REPORTS).ToList();
            }

            // Save to session
            var json = JsonSerializer.Serialize(recentlyViewed);
            HttpContext.Session.SetString(RECENTLY_VIEWED_KEY, json);

            return Ok();
        }

        private List<RecentReport> GetRecentlyViewedReports()
        {
            var json = HttpContext.Session.GetString(RECENTLY_VIEWED_KEY);
            if (string.IsNullOrEmpty(json))
            {
                return new List<RecentReport>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<RecentReport>>(json) ?? new List<RecentReport>();
            }
            catch
            {
                return new List<RecentReport>();
            }
        }

        private List<ReportCategory> GetCategories()
        {
            return new List<ReportCategory>
            {
                new ReportCategory { Name = "Sales", Icon = "fa-shopping-cart", Color = "success", Count = 7 },
                new ReportCategory { Name = "Purchase", Icon = "fa-truck", Color = "primary", Count = 6 },
                new ReportCategory { Name = "Inventory", Icon = "fa-boxes", Color = "warning", Count = 6 },
                new ReportCategory { Name = "Financial", Icon = "fa-dollar-sign", Color = "info", Count = 4 },
                new ReportCategory { Name = "Customer", Icon = "fa-users", Color = "purple", Count = 4 },
                new ReportCategory { Name = "Compliance", Icon = "fa-shield-alt", Color = "danger", Count = 1 }
            };
        }

        private List<ReportModel> GetAllReports()
        {
            return new List<ReportModel>
            {
                // SALES REPORTS (7)
                new ReportModel
                {
                    Title = "Sales Summary",
                    Description = "Overview of sales performance with revenue trends and comparisons",
                    Category = "Sales",
                    Icon = "fa-chart-line",
                    Url = "/Report/SaleSummary",
                    ExportFormats = new[] { "JSON" }
                },
                new ReportModel
                {
                    Title = "Sale by Product",
                    Description = "Product-wise sales analysis with quantities and revenue breakdown",
                    Category = "Sales",
                    Icon = "fa-pills",
                    Url = "/Report/SaleByProduct",
                    ExportFormats = new[] { "Preview", "PDF", "CSV" }
                },
                new ReportModel
                {
                    Title = "Sale by Customer",
                    Description = "Customer purchase history and total spend analysis",
                    Category = "Sales",
                    Icon = "fa-user-tag",
                    Url = "/Report/SaleByCustomer",
                    ExportFormats = new[] { "Preview", "PDF", "CSV" }
                },
                new ReportModel
                {
                    Title = "Monthly Sales Trends",
                    Description = "Month-over-month sales trending with visual charts",
                    Category = "Sales",
                    Icon = "fa-chart-area",
                    Url = "/Report/MonthlySalesTrends",
                    ExportFormats = new[] { "Chart" }
                },
                new ReportModel
                {
                    Title = "Sales Rep Performance",
                    Description = "Sales representative metrics and performance comparison",
                    Category = "Sales",
                    Icon = "fa-user-tie",
                    Url = "/Report/SalesRepPerformance",
                    ExportFormats = new[] { "Chart" }
                },
                new ReportModel
                {
                    Title = "Profit Margin Analysis",
                    Description = "Product profitability and margin analysis",
                    Category = "Sales",
                    Icon = "fa-percentage",
                    Url = "/Report/ProfitMargin",
                    ExportFormats = new[] { "Chart" }
                },
                new ReportModel
                {
                    Title = "Sales Dashboard",
                    Description = "Interactive sales analytics with multiple visualizations",
                    Category = "Sales",
                    Icon = "fa-tachometer-alt",
                    Url = "/Report/Sales",
                    ExportFormats = new[] { "Interactive" }
                },

                // PURCHASE REPORTS (6)
                new ReportModel
                {
                    Title = "Purchase Summary",
                    Description = "Overview of purchase orders and spending trends",
                    Category = "Purchase",
                    Icon = "fa-clipboard-list",
                    Url = "/Report/PurchaseSummary",
                    ExportFormats = new[] { "JSON" }
                },
                new ReportModel
                {
                    Title = "Purchase by Product",
                    Description = "Product-wise purchase analysis with costs and quantities",
                    Category = "Purchase",
                    Icon = "fa-pills",
                    Url = "/Report/PurchaseByProduct",
                    ExportFormats = new[] { "Preview", "PDF", "CSV" }
                },
                new ReportModel
                {
                    Title = "Purchase by Supplier",
                    Description = "Supplier-wise purchase history and total spend",
                    Category = "Purchase",
                    Icon = "fa-store",
                    Url = "/Report/PurchaseBySupplier",
                    ExportFormats = new[] { "Preview", "PDF" }
                },
                new ReportModel
                {
                    Title = "Purchase Trends",
                    Description = "Historical purchase patterns and forecasting",
                    Category = "Purchase",
                    Icon = "fa-chart-line",
                    Url = "/Report/PurchaseTrends",
                    ExportFormats = new[] { "Chart" }
                },
                new ReportModel
                {
                    Title = "Supplier Performance",
                    Description = "Supplier ratings, on-time delivery, and quality metrics",
                    Category = "Purchase",
                    Icon = "fa-star",
                    Url = "/Report/SupplierPerformance",
                    ExportFormats = new[] { "Dashboard" }
                },
                new ReportModel
                {
                    Title = "Expense Analysis",
                    Description = "Cost breakdown and expense categorization",
                    Category = "Purchase",
                    Icon = "fa-money-bill-wave",
                    Url = "/Report/ExpenseAnalysis",
                    ExportFormats = new[] { "Chart" }
                },

                // INVENTORY REPORTS (6)
                new ReportModel
                {
                    Title = "Stock Summary",
                    Description = "Current inventory levels and stock valuation",
                    Category = "Inventory",
                    Icon = "fa-warehouse",
                    Url = "/Report/StockSummary",
                    ExportFormats = new[] { "JSON" }
                },
                new ReportModel
                {
                    Title = "ABC Analysis",
                    Description = "Inventory classification by revenue contribution",
                    Category = "Inventory",
                    Icon = "fa-sort-amount-down",
                    Url = "/Report/ABCAnalysis",
                    ExportFormats = new[] { "Chart" }
                },
                new ReportModel
                {
                    Title = "Inventory Aging",
                    Description = "Age distribution of stock and slow-moving items",
                    Category = "Inventory",
                    Icon = "fa-clock",
                    Url = "/Report/InventoryAging",
                    ExportFormats = new[] { "Chart" }
                },
                new ReportModel
                {
                    Title = "Expiry Tracking",
                    Description = "Medicines expiring soon with urgency levels",
                    Category = "Inventory",
                    Icon = "fa-exclamation-triangle",
                    Url = "/Report/ExpiryTracking",
                    ExportFormats = new[] { "Dashboard" }
                },
                new ReportModel
                {
                    Title = "Inventory Optimization",
                    Description = "Reorder recommendations and optimization insights",
                    Category = "Inventory",
                    Icon = "fa-cogs",
                    Url = "/Report/InventoryOptimization",
                    ExportFormats = new[] { "Dashboard" }
                },
                new ReportModel
                {
                    Title = "Inventory Dashboard",
                    Description = "Comprehensive inventory analytics and visualizations",
                    Category = "Inventory",
                    Icon = "fa-chart-pie",
                    Url = "/Report/Inventory",
                    ExportFormats = new[] { "Interactive" }
                },

                // FINANCIAL REPORTS (4)
                new ReportModel
                {
                    Title = "Accounts Receivable",
                    Description = "Outstanding customer payments and aging analysis",
                    Category = "Financial",
                    Icon = "fa-file-invoice-dollar",
                    Url = "/Report/AccountsReceivableReport",
                    ExportFormats = new[] { "PDF", "CSV", "JSON" }
                },
                new ReportModel
                {
                    Title = "Accounts Payable",
                    Description = "Outstanding supplier payments and payment schedule",
                    Category = "Financial",
                    Icon = "fa-hand-holding-usd",
                    Url = "/Report/AccountsPayableReport",
                    ExportFormats = new[] { "PDF", "CSV", "JSON" }
                },
                new ReportModel
                {
                    Title = "Cash Flow Management",
                    Description = "30-day cash flow forecast with inflow/outflow analysis",
                    Category = "Financial",
                    Icon = "fa-chart-line",
                    Url = "/Report/CashFlowManagement",
                    ExportFormats = new[] { "Dashboard" }
                },
                new ReportModel
                {
                    Title = "Profit & Loss",
                    Description = "Revenue, expenses, and profitability analysis",
                    Category = "Financial",
                    Icon = "fa-balance-scale",
                    Url = "/Report/ProfitMargin",
                    ExportFormats = new[] { "Chart" }
                },

                // CUSTOMER ANALYTICS (4)
                new ReportModel
                {
                    Title = "Customer Lifetime Value",
                    Description = "CLV distribution and high-value customer identification",
                    Category = "Customer",
                    Icon = "fa-gem",
                    Url = "/Report/CustomerLifetimeValue",
                    ExportFormats = new[] { "Chart" }
                },
                new ReportModel
                {
                    Title = "Customer Retention",
                    Description = "Retention rates and churn analysis",
                    Category = "Customer",
                    Icon = "fa-user-check",
                    Url = "/Report/CustomerRetention",
                    ExportFormats = new[] { "Chart" }
                },
                new ReportModel
                {
                    Title = "Customer Segmentation",
                    Description = "Customer grouping by behavior and value",
                    Category = "Customer",
                    Icon = "fa-layer-group",
                    Url = "/Report/CustomerSegmentation",
                    ExportFormats = new[] { "Chart" }
                },
                new ReportModel
                {
                    Title = "Customer Intelligence",
                    Description = "Comprehensive customer analytics and insights",
                    Category = "Customer",
                    Icon = "fa-brain",
                    Url = "/Report/CustomerIntelligence",
                    ExportFormats = new[] { "Dashboard" }
                },

                // COMPLIANCE (1)
                new ReportModel
                {
                    Title = "Regulatory Compliance",
                    Description = "Pharmaceutical compliance tracking and audit reports",
                    Category = "Compliance",
                    Icon = "fa-file-medical-alt",
                    Url = "/Report/RegulatoryCompliance",
                    ExportFormats = new[] { "Planned" }
                }
            };
        }
    }

    #region Models

    public class ReportModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string[] ExportFormats { get; set; }
    }

    public class ReportCategory
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public int Count { get; set; }
    }

    public class RecentReport
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime ViewedAt { get; set; }
    }

    #endregion
}
