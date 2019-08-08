namespace Transaction.Domain.Entities
{
    /// <summary>
    /// Abstract base class model.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Entity Id.
        /// </summary>
        public virtual ulong Id { get; set; }
    }
}
