using System.ComponentModel;

namespace Pharma263.MVC.Enums
{
    public enum ReturnDestination
    {
        [Description("Stock Update")]
        StockUpdate = 1,

        [Description("Quarantine")]
        Quarantine,

        [Description("Disposal")]
        Disposal,

        [Description("Supplier")]
        Supplier
    }
}
