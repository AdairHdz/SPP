using DataPersistenceLayer.Entities;
using FluentValidation;
using System.Text.RegularExpressions;

namespace PresentationLayer.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithState(user => "TextBoxFirstName")
                .MinimumLength(1).WithState(user => "TextBoxFirstName").
                MaximumLength(50).Matches("^[a-zA-Z ]+$").WithState(user => "TextBoxFirstName");
            RuleFor(user => user.LastName).NotEmpty().WithState(user => "TextBoxLastName")
                .MaximumLength(30).WithState(user => "TextBoxLastName")
                .Matches("^[a-zA-Z ]+$").WithState(user => "TextBoxLastName");
            RuleFor(user => user.Gender).IsInEnum();
            RuleFor(user => user.UserStatus).IsInEnum();
            RuleFor(user => user.UserType).IsInEnum();
            RuleFor(user => user.PhoneNumber).NotEmpty().WithState(user => "TextBoxPhoneNumber")
                .Matches("^[0-9]{10}$").WithState(user => "TextBoxPhoneNumber");
            RuleFor(user => user.Email).Must(BeValidEmail)
                .WithState(user => "TextBoxEmail");
            RuleFor(user => user.AlternateEmail).Must(BeValidEmail).
                Unless(e => e.AlternateEmail == null || e.AlternateEmail.Length == 0).WithState(s => "TextBoxAlternateEmail");
            
        }

        public bool BeValidEmail(string email)
        {
            Regex regularExpression = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
            if(email == null || email.Length == 0)
            {
                return false;
            }
            bool hasValidFormat = regularExpression.IsMatch(email);
            bool hasValidLength = email.Length <= 254;
            return hasValidFormat && hasValidLength;
        }
    }
}
