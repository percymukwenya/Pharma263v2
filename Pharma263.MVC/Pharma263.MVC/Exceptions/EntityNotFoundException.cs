using System;

namespace Pharma263.MVC.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity is not found in the database
    /// Example: Customer with ID 123 not found
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public string EntityType { get; }
        public object EntityId { get; }

        public EntityNotFoundException(string entityType, object entityId)
            : base($"{entityType} with ID '{entityId}' was not found.")
        {
            EntityType = entityType;
            EntityId = entityId;
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
