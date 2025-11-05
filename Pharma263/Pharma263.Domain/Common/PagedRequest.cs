using System.ComponentModel.DataAnnotations;

namespace Pharma263.Domain.Common
{
    public class PagedRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10;

        public string SearchTerm { get; set; }

        public string SortBy { get; set; }

        public bool SortDescending { get; set; } = false;
    }
}