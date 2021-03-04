using DataPersistenceLayer.Entities;
using FluentValidation;

namespace PresentationLayer.Validators
{
    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            RuleFor(account => account.Username).NotEmpty()
                .WithState(account => "TextBoxUsername").MaximumLength(50);
            RuleFor(account => account.Password).NotEmpty()
                .WithState(account => "PasswordBoxPassword")
                .MaximumLength(50).MinimumLength(8);
        }
    }
}
