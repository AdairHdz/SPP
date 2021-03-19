using DataPersistenceLayer.Entities;
using FluentValidation;
using System.Text.RegularExpressions;

namespace PresentationLayer.Validators
{
    public class LinkedOrganizationValidator : AbstractValidator<LinkedOrganization>
    {
        public LinkedOrganizationValidator()
        {
            RuleFor(linkedOrganization => linkedOrganization.Name).NotEmpty()
                .MinimumLength(3).MaximumLength(50).Matches("[a-zA-Z+]")
                .WithState(linkedOrganization => "TextBoxName");

            RuleFor(linkedOrganization => linkedOrganization.Address).NotEmpty()
                .MinimumLength(3).MaximumLength(100).Matches("[a-zA-Z+]")
                .WithState(linkedOrganization => "TextBoxAddress");

            UserValidator userValidator = new UserValidator();
            RuleFor(linkedOrganization => linkedOrganization.Email)
                .MinimumLength(3).MaximumLength(100)
                .Must(userValidator.BeValidEmail).WithState(linkedOrganization => "TextBoxEmail");

            RuleFor(linkedOrganization => linkedOrganization.DirectUsers).NotEmpty()
                .GreaterThan(0).WithState(linkedOrganization => "TextBoxDirectUsers");

            RuleFor(linkedOrganization => linkedOrganization.IndirectUsers).NotEmpty()
                .GreaterThan(0).WithState(linkedOrganization => "TextBoxIndirectUsers");

            RuleFor(linkedOrganization => linkedOrganization.IdCity).NotEmpty()
                .GreaterThan(0);

            RuleFor(linkedOrganization => linkedOrganization.IdState).NotEmpty()
                .GreaterThan(0);

            RuleFor(linkedOrganization => linkedOrganization.IdSector).NotEmpty()
                .GreaterThan(0);
        }
    }
}