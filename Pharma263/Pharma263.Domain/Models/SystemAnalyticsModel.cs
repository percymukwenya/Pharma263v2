using System;
using System.Collections.Generic;

namespace Pharma263.Domain.Models
{
    /// <summary>
    /// System analytics and performance metrics
    /// </summary>
    public class SystemAnalyticsModel
    {
        public DateTime ReportDate { get; set; }
        public List<UserLoginActivityModel> UserLoginActivity { get; set; } = new List<UserLoginActivityModel>();
        public List<ApiUsageMetricModel> ApiUsageMetrics { get; set; } = new List<ApiUsageMetricModel>();
        public List<ErrorLogSummaryModel> ErrorSummary { get; set; } = new List<ErrorLogSummaryModel>();
        public SystemPerformanceModel Performance { get; set; }
    }

    public class UserLoginActivityModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime LastLogin { get; set; }
        public int LoginCount { get; set; }
        public TimeSpan AverageSessionDuration { get; set; }
        public string LastLoginIP { get; set; }
        public bool IsActive { get; set; }
    }

    public class ApiUsageMetricModel
    {
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public int RequestCount { get; set; }
        public double AverageResponseTime { get; set; }
        public int ErrorCount { get; set; }
        public double ErrorRate { get; set; }
        public DateTime PeakUsageTime { get; set; }
    }

    public class ErrorLogSummaryModel
    {
        public string ErrorType { get; set; }
        public string Module { get; set; }
        public int Count { get; set; }
        public DateTime LastOccurrence { get; set; }
        public string Severity { get; set; }
        public List<string> RecentMessages { get; set; } = new List<string>();
    }

    public class SystemPerformanceModel
    {
        public double CpuUsagePercent { get; set; }
        public double MemoryUsagePercent { get; set; }
        public double DiskUsagePercent { get; set; }
        public long DatabaseSize { get; set; }
        public double AverageQueryTime { get; set; }
        public int ActiveConnections { get; set; }
        public DateTime LastHealthCheck { get; set; }
        public string SystemStatus { get; set; }
    }
}