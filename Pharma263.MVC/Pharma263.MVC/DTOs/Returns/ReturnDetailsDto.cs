using System;

namespace Pharma263.MVC.DTOs.Returns
{
    public class ReturnDetailsDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string ReturnDestination { get; set; }
        public string ReturnReason { get; set; }
        public string ReturnStatus { get; set; }
        public string Medicine { get; set; }
        public int SaleId { get; set; }
        public DateTime DateReturned { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
