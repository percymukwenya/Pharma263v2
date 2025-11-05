using Pharma263.Application.Models;

namespace Pharma263.Application.Services.Printing
{
    public class PurchaseInvoiceReport : BaseReport<PurchaseReportViewModel>
    {
        public PurchaseInvoiceReport() : base(totalColumns: 6)
        {
        }

        protected override float[] GetColumnWidths()
        {
            // Purchase invoice columns: Item, Description, Qty, Unit Price, Discount, Total
            return new float[] { 80f, 200f, 80f, 100f, 80f, 100f };
        }

        protected override void BuildReportSpecificHeader()
        {
            throw new System.NotImplementedException();
        }

        protected override void BuildTableHeaders()
        {
            throw new System.NotImplementedException();
        }

        protected override void BuildTableBody()
        {
            throw new System.NotImplementedException();
        }
    }
}
