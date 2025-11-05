using Pharma263.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Pharma263.Application.Services
{
    public interface IReportAnalyticsService
    {
        /// <summary>
        /// Generates comprehensive dashboard summary with key business metrics
        /// </summary>
        Task<DashboardSummaryModel> GenerateDashboardSummary();

        /// <summary>
        /// Generates system analytics including user activity and performance metrics
        /// </summary>
        Task<SystemAnalyticsModel> GenerateSystemAnalytics(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Generates advanced business insights with trends and recommendations
        /// </summary>
        Task<BusinessInsightsModel> GenerateBusinessInsights(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Invalidates analytics cache for fresh data
        /// </summary>
        void InvalidateAnalyticsCache();
    }
}