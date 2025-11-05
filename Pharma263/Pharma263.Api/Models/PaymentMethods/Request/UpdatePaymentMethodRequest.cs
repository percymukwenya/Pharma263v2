namespace Pharma263.Api.Models.PaymentMethods.Request
{
    public class UpdatePaymentMethodRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
