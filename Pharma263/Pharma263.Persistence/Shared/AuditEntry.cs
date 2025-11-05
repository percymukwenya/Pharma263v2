using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System;

namespace Pharma263.Persistence.Shared
{
    public class AuditEntry
    {
        public long Id { get; set; }
        public string EntityName { get; set; }
        public string ActionType { get; set; }
        public string Username { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string EntityId { get; set; }
        public Dictionary<string, object> Changes { get; set; }

        public List<PropertyEntry> TempProperties { get; set; }

        public AuditEntry()
        {
            
        }
    }
}
