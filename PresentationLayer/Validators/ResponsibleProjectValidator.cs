
using DataPersistenceLayer.Entities;
using FluentValidation;
using System.Text.RegularExpressions;

namespace PresentationLayer.Validators
{
    public class ResponsibleProjectValidator : AbstractValidator<ResponsibleProject>
	{
		public ResponsibleProjectValidator()
        {
			RuleFor(responsibleProject => responsibleProject.Name).NotEmpty()
			   .MinimumLength(3).MaximumLength(50).Matches("[a-zA-Z+]").WithState(responsibleProject => "TextBoxName");
			RuleFor(responsibleProject => responsibleProject.LastName).NotEmpty()
				.MinimumLength(3).MaximumLength(50).Matches("[a-zA-Z+]").WithState(responsibleProject => "TextBoxLastName");
			UserValidator userValidator = new UserValidator();
			RuleFor(responsibleProject => responsibleProject.EmailAddress).Must(userValidator.BeValidEmail)
			   .WithState(responsibleProject => "TextBoxEmail");
			RuleFor(responsibleProject => responsibleProject.Charge).NotEmpty()
				.MinimumLength(3).MaximumLength(50).Matches("[a-zA-Z+]").WithState(responsibleProject => "TextBoxCharge");
		}
	}
}
