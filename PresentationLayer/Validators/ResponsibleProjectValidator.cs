
using DataPersistenceLayer.Entities;
using FluentValidation;

namespace PresentationLayer.Validators
{
    public class ResponsibleProjectValidator : AbstractValidator<ResponsibleProject>
	{
		public ResponsibleProjectValidator()
        {
			RuleFor(responsibleProject => responsibleProject.Name).NotEmpty().WithState(responsibleProject => "TextBoxName")
			   .MinimumLength(3).WithState(responsibleProject => "TextBoxName")
			   .MaximumLength(50).WithState(responsibleProject => "TextBoxName")
			   .Matches("[a-zA-Z+]").WithState(responsibleProject => "TextBoxName");

			RuleFor(responsibleProject => responsibleProject.LastName).NotEmpty().WithState(responsibleProject => "TextBoxLastName")
				.MinimumLength(3).WithState(responsibleProject => "TextBoxLastName")
				.MaximumLength(50).WithState(responsibleProject => "TextBoxLastName")
				.Matches("[a-zA-Z+]").WithState(responsibleProject => "TextBoxLastName");

			UserValidator userValidator = new UserValidator();
			RuleFor(responsibleProject => responsibleProject.EmailAddress).Must(userValidator.BeValidEmail)
			   .WithState(responsibleProject => "TextBoxEmail");

			RuleFor(responsibleProject => responsibleProject.Charge).NotEmpty().WithState(responsibleProject => "TextBoxCharge")
				.MinimumLength(3).WithState(responsibleProject => "TextBoxCharge")
				.MaximumLength(50).WithState(responsibleProject => "TextBoxCharge")
				.Matches("[a-zA-Z+]").WithState(responsibleProject => "TextBoxCharge");
		}
	}
}
