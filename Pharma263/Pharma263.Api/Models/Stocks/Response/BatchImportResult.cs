using System.Collections.Generic;

namespace Pharma263.Api.Models.Stocks.Response
{
    public class BatchImportResult
    {
        public int TotalRows { get; set; }
        public int SuccessfulImports { get; set; }
        public int FailedImports { get; set; }
        public List<string> Errors { get; set; }
    }
}
