using System.ComponentModel;

namespace Pharma263.Domain.Enums
{
    public enum PaymentMethods
    {
        [Description("Cash - USD")]
        Cash = 1,

        [Description("Cash - ZWL")]
        Bond,

        [Description("RTGS")]
        RTGS,

        [Description("EcoCash")]
        EcoCash
    }
}
