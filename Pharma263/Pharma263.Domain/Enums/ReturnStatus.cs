using System.ComponentModel;

namespace Pharma263.Domain.Enums
{
    public enum ReturnStatus
    {
        [Description("Processed")]
        Processed = 1,

        [Description("Decision Pending")]
        DecisionPending,

        [Description("Quarantined")]
        Quarantined,

        [Description("Disposed")]
        Disposed,

        [Description("Returned To Supplier")]
        ReturnedToSupplier
    }
}
