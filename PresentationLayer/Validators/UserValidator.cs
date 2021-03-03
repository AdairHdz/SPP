using DataPersistenceLayer.Entities;
using FluentValidation;
using System.Text.RegularExpressions;

namespace PresentationLayer.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().MaximumLength(50).Matches("[a-zA-Z+]");
            RuleFor(user => user.LastName).NotEmpty().MaximumLength(30).Matches("[a-zA-Z+]");
            RuleFor(user => user.Gender).IsInEnum();
            RuleFor(user => user.UserStatus).IsInEnum();
            RuleFor(user => user.UserType).IsInEnum();
            RuleFor(user => user.PhoneNumber).Length(10).Matches("[0-9+]");
            RuleFor(user => user.Email).Must(BeValidEmail);
            RuleFor(user => user.AlternateEmail).Custom((alternateEmail, context) =>
            {
                if(alternateEmail != null && alternateEmail.Length != 0)
                {
                    if (!BeValidEmail(alternateEmail))
                    {
                        context.AddFailure("Datos no válidos");
                    }
                }
            });
            RuleFor(user => user.PhoneNumber).Length(10);
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
