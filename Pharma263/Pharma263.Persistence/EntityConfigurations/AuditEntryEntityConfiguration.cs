using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Pharma263.Persistence.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharma263.Persistence.EntityConfigurations
{
    public class AuditEntryEntityConfiguration : IEntityTypeConfiguration<AuditEntry>
    {
        public void Configure(EntityTypeBuilder<AuditEntry> builder)
        {
            builder.ToTable("AuditEntry", "Pharma263");
            builder.HasKey(c => c.Id);
            builder.Property(x => x.EntityName).HasColumnName("EntityName").IsRequired(false);
            builder.Property(x => x.ActionType).HasColumnName("ActionType").IsRequired(false);
            builder.Property(x => x.Username).HasColumnName("Username").IsRequired(false);
            builder.Property(x => x.TimeStamp).HasColumnName("TimeStamp").IsRequired();
            builder.Property(x => x.EntityId).HasColumnName("EntityId").IsRequired(false);
            builder.Property(x => x.Changes).HasColumnName("Changes").HasConversion(
                value => JsonConvert.SerializeObject(value),
                serializedValue => JsonConvert.DeserializeObject<Dictionary<string, object>>(serializedValue),
                new ValueComparer<Dictionary<string, object>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c
                )
            ).IsRequired(false);

            builder.Ignore(x => x.TempProperties);

            //Indexes

            builder.HasIndex(u => new { u.EntityName, u.EntityId, u.ActionType }, "IX_AuditEntry_EntityName_EntityId_ActionType").IsUnique(false);
            builder.HasIndex(u => u.Username, "IX_AuditEntry_Username").IsUnique(false);
        }
    }
}
