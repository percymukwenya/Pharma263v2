using System.Collections.Generic;

namespace Pharma263.Integration.Api.Models.Common
{
    public class DataTableResponse<T>
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<T> Data { get; set; }
        public string Error { get; set; }
        
        public DataTableResponse()
        {
            Data = new List<T>();
        }
    }
}