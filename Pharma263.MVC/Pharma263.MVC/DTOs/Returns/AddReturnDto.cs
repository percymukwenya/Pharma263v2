using Pharma263.MVC.Enums;
using System;

namespace Pharma263.MVC.DTOs.Returns
{
    public class AddReturnDto
    {
        public int Quantity { get; set; }
        public ReturnDestination ReturnDestination { get; set; }
        public ReturnReason ReturnReason { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
        public int MedicineId { get; set; }
        public int SaleId { get; set; }
        public DateTime DateReturned { get; set; }
    }
}
