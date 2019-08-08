using System.Linq;
using Transaction.Domain.Entities;
using Transaction.Domain.Enum;
using Transaction.Infra.Repository;

namespace Transaction.Infra.Factory
{
    /// <summary>
    /// Initialize in memory repositories.
    /// </summary>
    public class RepositoryFactory
    {
        private readonly AccountRepository _accountRepository;
        private readonly AccountTransactionRepository _accountTransactionRepository;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="accountRepository"><see cref="AccountRepository"/> reference, received via dependency injection.</param>
        /// <param name="AccountTransactionRepository"><see cref="AccountTransactionRepository"/> reference, received via dependency injection.</param>
        public RepositoryFactory(AccountRepository accountRepository,
            AccountTransactionRepository accountTransactionRepository)
        {
            _accountRepository = accountRepository;
            _accountTransactionRepository = accountTransactionRepository;

            SetupAccountRepository();
            SetupAccountTransactionRepository();
        }

        private void SetupAccountRepository()
        {
            if(_accountRepository != null)
            {
                if (_accountRepository.Data.Count() == 0)
                {
                    // Create a new Accounts if collection is empty,
                    // which means you can't delete all Accounts.
                    // because on application startup, it will create below accounts as sample.
                    _accountRepository.Data.Add(new Account { AccountNumber = "account_a", Balance = 1900 });
                    _accountRepository.Data.Add(new Account { AccountNumber = "account_b", Balance = 1100 });
                    _accountRepository.SaveChanges();
                }
            }
        }

        private void SetupAccountTransactionRepository()
        {
            if(_accountTransactionRepository != null)
            {
                var hasAccounts = (_accountRepository.Data.Count() > 1);
                var hasTransactions = (_accountTransactionRepository.Data.Count() > 0);

                if (hasAccounts && !hasTransactions)
                {
                    //Check for sample created accounts.
                    var account1 = _accountRepository.Data.First();
                    var account2 = _accountRepository.Data.Last();
                    if (account1 != null && account2 != null)
                    {
                        // Create a new Transaction if collection is empty.
                        var transactionSample = new AccountTransaction()
                        {
                            OriginAccount = account1.AccountNumber,
                            DestinationAccount = account2.AccountNumber,
                            Amount = 100,
                            Type = TransactionType.Transfer
                        };
                        // which means you can't delete all Transactions.
                        // because on application startup, it will create below transaction as sample.
                        _accountTransactionRepository.Data.Add(transactionSample);
                        _accountTransactionRepository.SaveChanges();
                    }
                }
            }
        }
    }
}
