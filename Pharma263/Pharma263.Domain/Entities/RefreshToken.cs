using Pharma263.Domain.Common;
using System;

namespace Pharma263.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? RevokedDate { get; set; }
        public string RevokedByIp { get; set; }
        public string CreatedByIp { get; set; }
        public string ReplacedByToken { get; set; }

        // Navigation property
        public virtual ApplicationUser User { get; set; }

        // Helper properties
        public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
