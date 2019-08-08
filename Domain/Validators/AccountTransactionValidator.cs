using FluentValidation;
using System;
using Transaction.Domain.Entities;

namespace Transaction.Domain.Validators
{
    /// <summary>
    /// Validate <see cref="Account"/> Entity
    /// </summary>
    public class AccountTransactionValidator : AbstractValidator<AccountTransaction>
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public AccountTransactionValidator()
        {
            RuleFor(c => c)
                .NotNull()
                .OnAnyFailure(x =>
                {
                    throw new ArgumentNullException("Can't found the object.");
                });

            RuleFor(c => c.DestinationAccount)
                .NotEmpty().WithMessage("Wrong Account.DestinationAccount data formatting.")
                .NotNull().WithMessage("Account.DestinationAccount cannot be null.");

            RuleFor(c => c.OriginAccount)
                .NotEmpty().WithMessage("Wrong Account.OriginAccount data formatting.")
                .NotNull().WithMessage("Account.OriginAccount cannot be null.");
            
            RuleFor(c => c.Amount)
                .GreaterThan(0).WithMessage("Wrong Account.Amount data formatting.")
                .NotNull().WithMessage("Account.Amount cannot be null.");

            RuleFor(c => c.Date)
                .NotEmpty().WithMessage("Wrong Account.Date data formatting.")
                .NotNull().WithMessage("Account.Date cannot be null.");
        }
    }
}
