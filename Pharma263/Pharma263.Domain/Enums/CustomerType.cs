using System.ComponentModel;

namespace Pharma263.Domain.Enums
{
    public enum CustomerType
    {
        [Description("Pharmacy")]
        Pharmacy = 1,

        [Description("Doctor")]
        Doctor,

        [Description("Clinic")]
        Clinic,

        [Description("Shops")]
        Shops
    }
}
