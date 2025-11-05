using Pharma263.Domain.Common;
using System;

namespace Pharma263.Domain.Entities
{
    public class CustomerType : ConcurrencyTokenEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public CustomerType() : base()
        {            
        }

        public CustomerType(string name, string description) : this()
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(description, nameof(description));

            Name = name;
            Description = description;
        }
    }
}
