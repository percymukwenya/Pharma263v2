using System.ComponentModel;

namespace Pharma263.MVC.Enums
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
