using System;
using Transaction.API.Handlers;
using Transaction.Domain.Entities;
using Xunit;

namespace Transaction.API.Tests.Handlers
{
    public class AccountHandlerTests
    {
        private readonly AccountsHandler _accountsHandler = new AccountsHandler();

        [Fact]
        public void HandleValidAccount()
        {
            var account = new Account()
            {
                AccountNumber = "account_test",
                Balance = 0,
                Id = 1
            };

            Assert.NotNull(_accountsHandler.Handle(account));
        }

        [Fact]
        public void HandleInvalidAccount()
        {
            var account = new Account()
            {
                AccountNumber = String.Empty,
                Balance = 1
            };

            Assert.Null(_accountsHandler.Handle(account));
        }
    }
}
