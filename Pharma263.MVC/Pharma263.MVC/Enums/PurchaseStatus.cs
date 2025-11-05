using System.ComponentModel;

namespace Pharma263.MVC.Enums
{
    public enum PurchaseStatus
    {
        [Description("Due")]
        Due = 1,

        [Description("Partially Paid")]
        PartiallyPaid,

        [Description("Fully Paid")]
        FullyPaid
    }
}
