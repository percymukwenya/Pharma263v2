using System.ComponentModel;

namespace Pharma263.Domain.Enums
{
    public enum SaleStatus
    {
        [Description("Due - 7 days")]
        DueInSeven = 1,

        [Description("Due - 14 days")]
        DueInFourteen,

        [Description("Due - 30 days")]
        DueInThirty,

        [Description("Partially Paid")]
        PartiallyPaid,

        [Description("Fully Paid")]
        FullyPaid
    }
}
