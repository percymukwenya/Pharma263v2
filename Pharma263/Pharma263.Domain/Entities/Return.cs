using Pharma263.Domain.Common;
using System;

namespace Pharma263.Domain.Entities
{
    public class Returns : ConcurrencyTokenEntity, IAuditable
    {
        public int Quantity { get; set; }
        public DateTime DateReturned { get; set; }
        public string Notes { get; set; }

        public int ReturnDestinationId { get; set; }
        public ReturnDestination ReturnDestination { get; set; }

        public int ReturnReasonId { get; set; }
        public ReturnReason ReturnReason { get; set; }

        public int ReturnStatusId { get; set; }
        public ReturnStatus ReturnStatus { get; set; }

        public int StockId { get; set; }
        public Stock Stock { get; set; }

        public int SaleId { get; set; }
        public Sales Sale { get; set; }



        public Returns() : base()
        {
        }

        public Returns(int quantity, DateTime dateReturned, string notes, int returnDestinationId, int returnReasonId,
            int returnStatusId, int stockId, int saleId) : this()
        {
            Quantity = quantity;
            DateReturned = dateReturned;
            Notes = notes;
            ReturnDestinationId = returnDestinationId;
            ReturnReasonId = returnReasonId;
            ReturnStatusId = returnStatusId;
            SaleId = saleId;
            StockId = stockId;
        }

        public Returns(ReturnDestination returnDestination, ReturnReason returnReason, ReturnStatus returnStatus, Stock stock, Sales sale, int quantity, DateTime dateReturned, string notes) : this()
        {
            ReturnDestination = returnDestination;
            ReturnDestinationId = returnDestination.Id;

            ReturnReason = returnReason;
            ReturnReasonId = returnReason.Id;

            ReturnStatus = returnStatus;
            ReturnStatusId = returnStatus.Id;

            Sale = sale;
            SaleId = sale.Id;

            Stock = stock;
            StockId = stock.Id;

            Quantity = quantity;
            DateReturned = dateReturned;
            Notes = notes;
        }
    }
}
