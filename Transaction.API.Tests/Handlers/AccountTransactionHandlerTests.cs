using System;
using Transaction.API.Handlers;
using Transaction.Domain.Entities;
using Transaction.Domain.Enum;
using Xunit;

namespace Transaction.API.Tests.Handlers
{
    public class AccountTransactionsHandlerTests
    {
        private readonly AccountTransactionsHandler _accountTransactionsHandler = new AccountTransactionsHandler();

        [Fact]
        public void HandleValidTransaction()
        {
            var transaction = new AccountTransaction()
            {
                OriginAccount = "oririn_account_test",
                DestinationAccount = "destination_account_test",
                Amount = 100,
                Type = TransactionType.Transfer
            };

            Assert.NotNull(_accountTransactionsHandler.Handle(transaction));
        }

        [Fact]
        public void HandleInvalidTransationFormat()
        {
            var transaction = new AccountTransaction()
            {
                OriginAccount = String.Empty,
                DestinationAccount = String.Empty,
                Amount = -1
            };

            Assert.Null(_accountTransactionsHandler.Handle(transaction));
        }

        [Fact]
        public void HandleInvalidAmount()
        {
            var transaction = new AccountTransaction()
            {
                OriginAccount = "oririn_account_test",
                DestinationAccount = "destination_account_test",
                Amount = -1,
                Type = TransactionType.Transfer
            };

            Assert.Null(_accountTransactionsHandler.Handle(transaction));
        }

        [Fact]
        public void CheckBalance()
        {
            var account = new Account
            {
                AccountNumber = "account_test",
                Balance = 1000f
            };

            var hasFunds = _accountTransactionsHandler
                .CheckBalance(account.Balance, 100);
            var noFunds = _accountTransactionsHandler
                .CheckBalance(account.Balance, 2000);

            Assert.True(hasFunds);
            Assert.False(noFunds);
        }

        [Theory]
        [InlineData(-50f)]
        [InlineData(20000f)]
        [InlineData(-1500f)]
        public void UpdateBalance(float value)
        {
            var account = new Account
            {
                AccountNumber = "account_test",
                Balance = 1000f
            };

            var expectedBalance = account.Balance + value;
            var result = _accountTransactionsHandler
                .UpdateBalance(account.Balance, value);

            Assert.Equal(expectedBalance, result);
        }
    }
}
