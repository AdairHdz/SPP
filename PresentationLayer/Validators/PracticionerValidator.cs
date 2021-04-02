using DataPersistenceLayer.Entities;
using FluentValidation;

namespace PresentationLayer.Validators
{
    public class PracticionerValidator : AbstractValidator<Practicioner>
    {
        public PracticionerValidator()
        {
           
            RuleFor(practicioner => practicioner.User.Account.Password).NotEmpty().WithState(account => "PasswordBoxPassword")
               .WithState(account => "PasswordBoxPassword").MaximumLength(60)
               .WithState(account => "PasswordBoxPassword").MinimumLength(8);
            ValidationGeneral();

        }

        public PracticionerValidator(bool modify)
        {
            ValidationGeneral();
        }

        private bool BeValidCredits(int credits)
        {
            bool isValid = true;
            if(credits < 285 || credits > 347)
            {
                isValid = false;
            }
            return isValid;
        }

        private void ValidationGeneral()
        {
            RuleFor(practicioner => practicioner.Enrollment).NotEmpty().Length(10).Matches("^[z]" + "[S]" + "[0-9]{8}$").
               WithState(practicioner => "TextBoxUsername");

            RuleFor(practicioner => practicioner.Credits).NotEmpty().Must(BeValidCredits).
                WithState(practicioner => "TextBoxCredits");

            RuleFor(practicioner => practicioner.Term).NotEmpty().WithState(practicioner => "ComboBoxPeriod");

            RuleFor(practicioner => practicioner.User).SetValidator(new UserValidator());
        }
    }
}
