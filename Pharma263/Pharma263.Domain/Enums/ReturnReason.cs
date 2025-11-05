using System.ComponentModel;

namespace Pharma263.Domain.Enums
{
    public enum ReturnReason
    {
        [Description("Over Ordering")]
        OverOrder = 1,

        [Description("Expired Product")]
        ExpiredProduct,

        [Description("Damaged")]
        Damaged,

        [Description("Factory Recall")]
        FactoryRecall,

        [Description("Other")]
        Other
    }
}
