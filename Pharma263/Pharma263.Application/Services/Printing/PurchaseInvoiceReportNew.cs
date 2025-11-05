using iTextSharp.text;
using iTextSharp.text.pdf;
using Pharma263.Application.Models;
using System.Globalization;

namespace Pharma263.Application.Services.Printing
{
    public class PurchaseInvoiceReportNew : BaseReport<PurchaseReportViewModel>
    {
        public PurchaseInvoiceReportNew() : base(totalColumns: 6)
        {
        }

        protected override float[] GetColumnWidths()
        {
            // Purchase invoice columns: SL No, Item, Price, Discount, Quantity, Amount
            return new float[] { 40f, 140f, 70f, 70f, 70f, 70f };
        }

        protected override void BuildReportSpecificHeader()
        {
            // Add company information
            AddCompanyInfo(_model.Company);

            // TO and Purchase Order headers (two-column layout like original)
            _fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _pdfPCell = new PdfPCell(new Phrase("TO : ", _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Purchase Order :  ", _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();

            // Supplier and Purchase Code
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(_model.Purchase.Supplier, _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(_model.Purchase.PurchaseCode.ToString(), _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();

            // Supplier Phone and Purchase Date
            _pdfPCell = new PdfPCell(new Phrase(_model.Purchase.SupplierPhoneNumber ?? "", _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(_model.Purchase.PurchaseDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), _fontStyle));
            _pdfPCell.Colspan = 1;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            paymentTable.AddCell(_pdfPCell);
            paymentTable.CompleteRow();
        }

        protected override void BuildTableHeaders()
        {
            // Table headers matching original PurchaseReport
            _pdfPTable.AddCell(CreateHeaderCell("SL No"));
            _pdfPTable.AddCell(CreateHeaderCell("Item"));
            _pdfPTable.AddCell(CreateHeaderCell("Price"));
            _pdfPTable.AddCell(CreateHeaderCell("Discount"));
            _pdfPTable.AddCell(CreateHeaderCell("Quantity"));
            _pdfPTable.AddCell(CreateHeaderCell("Amount"));
        }

        protected override void BuildTableBody()
        {
            // Table rows
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            decimal totalDiscount = 0;
            int serialNumber = 1;

            foreach (var item in _model.Purchase.Items)
            {
                _pdfPTable.AddCell(CreateStandardCell(serialNumber.ToString(), _fontStyle));
                _pdfPTable.AddCell(CreateStandardCell(item.MedicineName ?? "", _fontStyle));
                _pdfPTable.AddCell(CreateStandardCell(_model.Company.Currency + item.Price.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle));
                _pdfPTable.AddCell(CreateStandardCell(_model.Company.Currency + item.Discount.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle));
                _pdfPTable.AddCell(CreateStandardCell(item.Quantity.ToString(), _fontStyle));
                _pdfPTable.AddCell(CreateStandardCell(_model.Company.Currency + item.Amount.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle));
                
                totalDiscount += item.Discount;
                serialNumber++;
            }

            // Add spacing like original
            _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfPCell.Colspan = 6;
            _pdfPCell.Border = 0;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();

            // Summary rows using borderless style like original
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            
            // Total
            _pdfPTable.AddCell(CreateSummaryCell("Total :", _fontStyle, 5));
            _pdfPTable.AddCell(CreateSummaryCell(_model.Company.Currency + _model.Purchase.Total.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle, 1, Element.ALIGN_CENTER));
            _pdfPTable.CompleteRow();

            // Discount
            _pdfPTable.AddCell(CreateSummaryCell("Discount :", _fontStyle, 5));
            _pdfPTable.AddCell(CreateSummaryCell(_model.Company.Currency + totalDiscount.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle, 1, Element.ALIGN_CENTER));
            _pdfPTable.CompleteRow();

            // Grand Total (bold style)
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _pdfPTable.AddCell(CreateSummaryCell("Grand Total :", _fontStyle, 5));
            _pdfPTable.AddCell(CreateSummaryCell(_model.Company.Currency + _model.Purchase.GrandTotal.ToString("### ###0.00", CultureInfo.InvariantCulture), _fontStyle, 1, Element.ALIGN_CENTER));
            _pdfPTable.CompleteRow();

            // Status (after Grand Total like original)
            _pdfPTable.AddCell(CreateSummaryCell("Status  :", _fontStyle, 5));
            _pdfPTable.AddCell(CreateSummaryCell(_model.Purchase.PurchaseStatus ?? "Pending", _fontStyle, 1, Element.ALIGN_CENTER));
            _pdfPTable.CompleteRow();

            // Add spacing before footer like original
            for (int i = 0; i < 5; i++)
            {
                _pdfPCell = new PdfPCell(new Phrase(" ", _fontStyle));
                _pdfPCell.Colspan = 6;
                _pdfPCell.Border = 0;
                _pdfPTable.AddCell(_pdfPCell);
                _pdfPTable.CompleteRow();
            }

            // Footer section like original
            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _pdfPCell = new PdfPCell(new Phrase(" Thanks for your shoping.", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();

            _pdfPCell = new PdfPCell(new Phrase("Sales Invoice generated. ", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();
        }
    }
}