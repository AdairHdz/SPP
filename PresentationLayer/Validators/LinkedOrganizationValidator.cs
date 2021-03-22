using DataPersistenceLayer.Entities;
using FluentValidation;
using System.Collections.Generic;
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

            RuleForEach(linkedList => linkedList.PhoneNumbers).SetValidator(new PhoneValidator());

            UserValidator userValidator = new UserValidator();
            RuleFor(linkedOrganization => linkedOrganization.Email)
                .MinimumLength(3).MaximumLength(100)
                .Must(userValidator.BeValidEmail).WithState(linkedOrganization => "TextBoxEmail");

            RuleFor(linkedOrganization => linkedOrganization.DirectUsers).NotEmpty()
                .MinimumLength(1).MaximumLength(150).Matches("[0-9a-zA-Z+]")
                .WithState(linkedOrganization => "TextBoxDirectUsers");

            RuleFor(linkedOrganization => linkedOrganization.IndirectUsers).NotEmpty()
                .MinimumLength(1).MaximumLength(150).Matches("[0-9a-zA-Z+]")
                .WithState(linkedOrganization => "TextBoxIndirectUsers");

            RuleFor(linkedOrganization => linkedOrganization.IdCity).NotEmpty()
                .GreaterThan(0);

            RuleFor(linkedOrganization => linkedOrganization.IdState).NotEmpty()
                .GreaterThan(0);

            RuleFor(linkedOrganization => linkedOrganization.IdSector).NotEmpty()
                .GreaterThan(0);
        }
    }
}