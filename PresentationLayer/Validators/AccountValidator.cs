using DataPersistenceLayer.Entities;
using FluentValidation;

namespace PresentationLayer.Validators
{
    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            RuleFor(loginAccount => loginAccount.Username).NotEmpty()
                .MaximumLength(50).MinimumLength(1);
            RuleFor(loginAccount => loginAccount.Password).NotEmpty()
                .MaximumLength(50).MinimumLength(8);
        }
    }
}
