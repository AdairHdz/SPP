
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
			RuleFor(responsibleProject => responsibleProject.EmailAddress).Must(BeValidEmail)
			   .WithState(responsibleProject => "TextBoxEmail");
			RuleFor(responsibleProject => responsibleProject.Charge).NotEmpty()
				.MinimumLength(3).MaximumLength(50).Matches("[a-zA-Z+]").WithState(responsibleProject => "TextBoxCharge");
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
