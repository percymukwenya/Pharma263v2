namespace Pharma263.Domain.Common
{
    public class ConcurrencyTokenEntity : BaseEntity
    {
        public ConcurrencyTokenEntity() : base() { }

        public byte[] TimeStamp { get; set; }
    }
}
