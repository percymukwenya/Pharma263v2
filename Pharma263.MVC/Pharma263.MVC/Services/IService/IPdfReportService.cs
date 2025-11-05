using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;

namespace Pharma263.MVC.Services.IService
{
    public interface IPdfReportService
    {
        /// <summary>
        /// Generate a standardized PDF report
        /// </summary>
        /// <param name="title">Report title</param>
        /// <param name="data">Report data as enumerable</param>
        /// <param name="startDate">Optional start date for date range</param>
        /// <param name="endDate">Optional end date for date range</param>
        /// <param name="customColumns">Optional custom column headers</param>
        /// <returns>PDF as byte array</returns>
        byte[] GenerateStandardReport<T>(string title, IEnumerable<T> data, string startDate = null, string endDate = null, Dictionary<string, string> customColumns = null);

        /// <summary>
        /// Generate a simple table PDF report
        /// </summary>
        /// <param name="title">Report title</param>
        /// <param name="headers">Table headers</param>
        /// <param name="rows">Table rows data</param>
        /// <param name="startDate">Optional start date</param>
        /// <param name="endDate">Optional end date</param>
        /// <returns>PDF as byte array</returns>
        byte[] GenerateTableReport(string title, string[] headers, List<string[]> rows, string startDate = null, string endDate = null);

        /// <summary>
        /// Create a new PDF document with standard branding
        /// </summary>
        /// <returns>Document and MemoryStream for manual PDF creation</returns>
        (Document document, MemoryStream stream) CreateStandardDocument();

        /// <summary>
        /// Add company logo to document
        /// </summary>
        /// <param name="document">PDF document</param>
        void AddLogo(Document document);

        /// <summary>
        /// Add standardized title and date information
        /// </summary>
        /// <param name="document">PDF document</param>
        /// <param name="title">Report title</param>
        /// <param name="startDate">Optional start date</param>
        /// <param name="endDate">Optional end date</param>
        void AddTitleAndDates(Document document, string title, string startDate = null, string endDate = null);

        /// <summary>
        /// Create a standardized table with consistent styling
        /// </summary>
        /// <param name="headers">Table headers</param>
        /// <param name="rows">Table rows</param>
        /// <returns>Formatted PDF table</returns>
        PdfPTable CreateStandardTable(string[] headers, List<string[]> rows);
    }
}