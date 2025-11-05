using Pharma263.Domain.Entities;
using System;
using System.Linq;

namespace Pharma263.Api.Services
{
    public interface IPurchaseCalculationService
    {
        decimal CalculateLineNetAmount(decimal price, int quantity, decimal discount);
        void RecalculateTotals(Purchase purchase);

    }

    public class PurchaseCalculationService : IPurchaseCalculationService
    {
        public decimal CalculateLineNetAmount(decimal price, int quantity, decimal discount)
        {
            var grossTotal = price * quantity;
            if (discount > grossTotal)
                throw new InvalidOperationException("Item discount cannot exceed the total price for that item.");
            return grossTotal - discount;
        }

        public void RecalculateTotals(Purchase purchase)
        {
            // Total is the sum of line gross amounts, GrandTotal is the sum of net amounts
            purchase.Total = purchase.Items.Sum(x => x.Price * x.Quantity);
            purchase.GrandTotal = purchase.Items.Sum(x => CalculateLineNetAmount(x.Price, x.Quantity, x.Discount));
        }

    }
}
