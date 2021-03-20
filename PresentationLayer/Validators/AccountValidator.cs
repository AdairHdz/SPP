using DataPersistenceLayer.Entities;
using FluentValidation;

namespace PresentationLayer.Validators
{
    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            RuleFor(account => account.Username).NotEmpty().WithState(account => "TextBoxUsername")
                .MaximumLength(50).WithState(account => "TextBoxUsername");
            RuleFor(account => account.Password).NotEmpty().WithState(account => "PasswordBoxPassword")
                .WithState(account => "PasswordBoxPassword").MaximumLength(60)
                .WithState(account => "PasswordBoxPassword").MinimumLength(8);
        }
    }
}
