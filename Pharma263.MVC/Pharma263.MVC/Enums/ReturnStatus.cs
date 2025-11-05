using System.ComponentModel;

namespace Pharma263.MVC.Enums
{
    public enum ReturnStatus
    {
        [Description("Decision Pending")]
        DecisionPending = 1,

        [Description("Quarantined")]
        Quarantined,

        [Description("Disposed")]
        Disposed,

        [Description("Returned To Supplier")]
        ReturnedToSupplier
    }
}
