using DataPersistenceLayer.Entities;
using FluentValidation;
using System.Text.RegularExpressions;

namespace PresentationLayer.Validators
{
    public class LinkedOrganizationValidator : AbstractValidator<LinkedOrganization>
    {
        public LinkedOrganizationValidator()
        {
            RuleFor(linkedOrganization => linkedOrganization.Name)
                .NotEmpty().WithState(linkedOrganization => "TextBoxName")
                .MaximumLength(150).WithState(linkedOrganization => "TextBoxName")
                .Matches("^[a-zA-Z ]+$").WithState(linkedOrganization => "TextBoxName");

            RuleFor(linkedOrganization => linkedOrganization.DirectUsers)
                .NotEmpty().WithState(linkedOrganization => "TextBoxDirectUsers")
                .MaximumLength(254).WithState(linkedOrganization => "TextBoxDirectUsers")
                .Matches("^[a-zA-Z ]+$").WithState(linkedOrganization => "TextBoxDirectUsers");

            RuleFor(linkedOrganization => linkedOrganization.IndirectUsers)
                .NotEmpty().WithState(linkedOrganization => "TextBoxIndirectUsers")
                .MaximumLength(254).WithState(linkedOrganization => "TextBoxIndirectUsers")
                .Matches("^[a-zA-Z ]+$").WithState(linkedOrganization => "TextBoxIndirectUsers");

            RuleFor(linkedOrganization => linkedOrganization.Email)
                .Must(BeValidEmail).WithState(linkedOrganization => "TextBoxEmail");

            RuleFor(linkedOrganization => linkedOrganization.Address)
                .NotEmpty().WithState(linkedOrganization => "TextBoxAddress")
                .MaximumLength(100).WithState(linkedOrganization => "TextBoxAddress");

            RuleFor(linkedOrganization => linkedOrganization.City).NotNull();
            RuleFor(linkedOrganization => linkedOrganization.State).NotNull();
            RuleFor(linkedOrganization => linkedOrganization.Sector).NotNull();

            RuleFor(linkedOrganization => linkedOrganization.PhoneNumbers[0]).SetValidator(new PhoneValidator(1));
            RuleFor(linkedOrganization => linkedOrganization.PhoneNumbers[1]).SetValidator(new PhoneValidator(2));
        }

        public bool BeValidEmail(string email)
        {
            Regex regularExpression = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
            if (email == null || email.Length == 0)
            {
                return false;
            }
            bool hasValidFormat = regularExpression.IsMatch(email);
            bool hasValidLength = email.Length <= 254;
            return hasValidFormat && hasValidLength;
        }
    }
}
