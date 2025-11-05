using Pharma263.MVC.Utility;
using static Pharma263.MVC.Utility.StaticDetails;

namespace Pharma263.MVC.DTOs
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }
    }
}
