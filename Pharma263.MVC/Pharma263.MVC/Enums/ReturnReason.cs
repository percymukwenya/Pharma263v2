using System.ComponentModel;

namespace Pharma263.MVC.Enums
{
    public enum ReturnReason
    {
        [Description("Expired Product")]
        ExpiredProduct = 1,

        [Description("Damaged")]
        Damaged,

        [Description("Factory Recall")]
        FactoryRecall,

        [Description("Other")]
        Other
    }
}
