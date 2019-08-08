using FluentValidation;
using System;
using Transaction.Domain.Entities;

namespace Transaction.Domain.Validators
{
    /// <summary>
    /// Validate <see cref="Account"/> Entity
    /// </summary>
    public class AccountValidator : AbstractValidator<Account>
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public AccountValidator()
        {
            RuleFor(c => c)
                .NotNull()
                .OnAnyFailure(x =>
                {
                    throw new ArgumentNullException("Can't found the object.");
                });

            RuleFor(c => c.AccountNumber)
                .NotEmpty().WithMessage("Wrong Account.AccountNumber data formatting.")
                .NotNull().WithMessage("Account.AccountNumber cannot be null.");

            RuleFor(c => c.Balance)
                .GreaterThanOrEqualTo(0).WithMessage("Wrong Account.Balance data formatting.")
                .NotNull().WithMessage("Account.Balance cannot be null.");
        }
    }
}
