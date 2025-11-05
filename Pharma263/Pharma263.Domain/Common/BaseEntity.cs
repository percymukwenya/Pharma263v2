using System;

namespace Pharma263.Domain.Common
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreatedDate = DateTimeOffset.Now;
        }

        public int Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        public void UpdateIsDelete(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }
    }
}
