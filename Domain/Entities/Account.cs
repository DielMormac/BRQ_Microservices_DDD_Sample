namespace Transaction.Domain.Entities
{
    /// <summary>
    /// Basic bank account sample.
    /// </summary>
    public class Account : BaseEntity
    {
        /// <summary>
        /// Account number.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Account Balance.
        /// </summary>
        public float Balance { get; set; }
    }
}
