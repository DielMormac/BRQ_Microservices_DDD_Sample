using Transaction.API.Interfaces;
using Transaction.Domain.Entities;
using Transaction.Domain.Validators;

namespace Transaction.API.Handlers
{
    /// <summary>
    /// Handle <see cref="Account"/> operations.
    /// </summary>
    public class AccountsHandler : IHandler<Account>
    {
        private readonly AccountValidator _accountValidator;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public AccountsHandler()
        {
            _accountValidator = new AccountValidator();
        }

        /// <inheritdoc/>
        public Account Handle(Account entity)
        {
            var result = _accountValidator.Validate(entity);

            if (!result.IsValid)
                return null;

            return entity;
        }
    }
}
