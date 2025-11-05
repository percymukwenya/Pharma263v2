using Pharma263.Domain.Common;
using System;

namespace Pharma263.Domain.Entities
{
    public class AccountsReceivableStatus : ConcurrencyTokenEntity, IAuditable
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public AccountsReceivableStatus() : base()
        {
        }

        public AccountsReceivableStatus(string name, string description) : this()
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(description, nameof(description));

            Name = name;
            Description = description;
        }
    }
}