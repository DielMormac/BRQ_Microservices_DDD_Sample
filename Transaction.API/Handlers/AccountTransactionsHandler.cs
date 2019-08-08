using Transaction.API.Interfaces;
using Transaction.Domain.Entities;
using Transaction.Domain.Enum;
using Transaction.Domain.Validators;

namespace Transaction.API.Handlers
{
    /// <summary>
    /// Handle <see cref="AccountTransactions"/> operations.
    /// </summary>
    public class AccountTransactionsHandler : IHandler<AccountTransaction>
    {
        private readonly AccountValidator _accountValidator;
        private readonly AccountTransactionValidator _accountTransactionValidator;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public AccountTransactionsHandler()
        {
            _accountValidator = new AccountValidator();
            _accountTransactionValidator = new AccountTransactionValidator();
        }

        /// <inheritdoc/>
        public AccountTransaction Handle(AccountTransaction entity)
        {
            var validator = _accountTransactionValidator.Validate(entity);

            if (!validator.IsValid)
                return null;

            if (entity.Type != TransactionType.Transfer)
                entity.OriginAccount = entity.DestinationAccount;

            return entity;
        }

        /// <summary>
        /// Update Account Balance.
        /// </summary>
        /// <param name="currentBalance">Current Balance.</param>
        /// <param name="debit">Amount to be debited.</param>
        /// <returns>Updated Balance.</returns>
        public float UpdateBalance(float currentBalance, float debit)
        {
            return currentBalance + debit;
        }

        /// <summary>
        /// Check if account balance has funds.
        /// </summary>
        /// <param name="currentBalance">Current Balance</param>
        /// <param name="debit">Amount ot be debited.</param>
        /// <returns>bool.</returns>
        public bool CheckBalance(float currentBalance, float debit)
        {
            return (debit <= currentBalance);
        }
    }
}
