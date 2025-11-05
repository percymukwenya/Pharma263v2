using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Pharma263.MVC.Models
{
    public class ProcessReturnViewModel
    {
        public int SaleId { get; set; }
        public string CustomerName { get; set; }
        public string SaleDate { get; set; }
        public decimal SaleTotal { get; set; }
        public string SaleStatus { get; set; }

        public SelectList ReturnReasonsSelectList { get; set; }
        public SelectList ReturnDestinationsSelectList { get; set; }
        public List<ReturnItemViewModel> ReturnItems { get; set; }
    }
}
