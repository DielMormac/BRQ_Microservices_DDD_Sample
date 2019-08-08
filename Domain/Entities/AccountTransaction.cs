using System;
using Transaction.Domain.Enum;

namespace Transaction.Domain.Entities
{
    /// <summary>
    /// Bank transation sample.
    /// </summary>
    public class AccountTransaction : BaseEntity
    {
        /// <summary>
        /// Origin <see cref="Account.AccountNumber"/>.
        /// </summary>
        public string OriginAccount { get; set; }

        /// <summary>
        /// Destination <see cref="Account.AccountNumber"/>.
        /// </summary>
        public string DestinationAccount { get; set; }

        /// <summary>
        /// Amount to be transfered.
        /// </summary>
        public float Amount { get; set; }

        /// <summary>
        /// Date of the transaction.
        /// </summary>
        public DateTime Date { get; set; } = DateTime.Now;

        /// <summary>
        /// <see cref="TransactionType"/>.
        /// </summary>
        public TransactionType Type { get; set; }
    }
}
