using Pharma263.Domain.Common;

namespace Pharma263.Domain.Entities
{
    public class SalesItems : ConcurrencyTokenEntity, IAuditable
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }

        public int StockId { get; set; }
        public Stock Stock { get; set; }

        public int SaleId { get; set; }
        public Sales Sale { get; set; }        
    }
}
