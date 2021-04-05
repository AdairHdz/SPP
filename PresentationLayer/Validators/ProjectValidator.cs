using DataPersistenceLayer.Entities;
using FluentValidation;

namespace PresentationLayer.Validators
{
	public class ProjectValidator : AbstractValidator<Project>
	{
		public ProjectValidator()
		{
			RuleFor(project => project.NameProject).NotEmpty().MinimumLength(6).WithState(project => "TextBoxName")
				.MaximumLength(150).WithState(project => "TextBoxName")
				.Matches("[a-zA-Z+]").WithState(project => "TextBoxName");

			RuleFor(project => project.Description).NotEmpty().WithState(project => "TextBoxDescriptionGeneral")
				.MinimumLength(5).WithState(project => "TextBoxDescriptionGeneral")
				.MaximumLength(300).WithState(project => "TextBoxDescriptionGeneral")
				.Matches("[a-zA-Z+]").WithState(project => "TextBoxDescriptionGeneral");

			RuleFor(project => project.ObjectiveGeneral).NotEmpty().WithState(project => "TextBoxObjectiveGeneral")
				.MinimumLength(5).WithState(project => "TextBoxObjectiveGeneral")
				.MaximumLength(300).WithState(project => "TextBoxObjectiveGeneral")
				.Matches("[a-zA-Z+]").WithState(project => "TextBoxObjectiveGeneral");

			RuleFor(project => project.ObjectiveImmediate).NotEmpty().WithState(project => "TextBoxObjectiveImmediate")
				.MinimumLength(5).WithState(project => "TextBoxObjectiveImmediate")
				.MaximumLength(300).WithState(project => "TextBoxObjectiveImmediate")
				.Matches("[a-zA-Z+]").WithState(project => "TextBoxObjectiveImmediate");

			RuleFor(project => project.ObjectiveMediate).NotEmpty().WithState(project => "TextBoxObjectiveMediate")
				.MinimumLength(5).WithState(project => "TextBoxObjectiveMediate")
				.MaximumLength(300).WithState(project => "TextBoxObjectiveMediate")
				.Matches("[a-zA-Z+]").WithState(project => "TextBoxObjectiveMediate");

			RuleFor(project => project.Methodology).NotEmpty().WithState(project => "TextBoxMethodology")
				.MinimumLength(5).WithState(project => "TextBoxMethodology")
				.MaximumLength(300).WithState(project => "TextBoxMethodology")
				.Matches("[a-zA-Z+]").WithState(project => "TextBoxMethodology");

			RuleFor(project => project.Resources).NotEmpty().WithState(project => "TextBoxResources")
				.MinimumLength(5).WithState(project => "TextBoxResources")
				.MaximumLength(300).WithState(project => "TextBoxResources")
				.Matches("[a-zA-Z+]").WithState(project => "TextBoxResources");

			RuleFor(project => project.Activities).NotEmpty().WithState(project => "TextBoxActivities")
				.MinimumLength(5).WithState(project => "TextBoxActivities")
				.MaximumLength(300).WithState(project => "TextBoxActivities")
				.Matches("[a-zA-Z+]").WithState(project => "TextBoxActivities");

			RuleFor(project => project.Responsibilities).NotEmpty().WithState(project => "TextBoxResponsibilities")
				.MinimumLength(5).WithState(project => "TextBoxResponsibilities")
				.MaximumLength(300).WithState(project => "TextBoxResponsibilities")
				.Matches("[a-zA-Z+]").WithState(project => "TextBoxResponsibilities");

			RuleFor(project => project.QuantityPracticing).NotEmpty().WithState(project => "TextBoxQuantityPracticing")
				.GreaterThan(0).WithState(project => "TextBoxQuantityPracticing")
				.LessThan(5).WithState(project => "TextBoxQuantityPracticing");

			RuleFor(project => project.DaysHours).NotEmpty().WithState(project => "TextBoxDaysHours")
				.MinimumLength(5).WithState(project => "TextBoxDaysHours")
				.MaximumLength(100).WithState(project => "TextBoxDaysHours")
				.Matches("[a-zA-Z+]").WithState(project => "TextBoxDaysHours");

			RuleFor(project => project.Term).NotEmpty().MinimumLength(5).MaximumLength(50).Matches("[a-zA-Z+]");
			RuleFor(project => project.Duration).NotEmpty().Equals(480);
			RuleFor(project => project.StaffNumberCoordinator).NotEmpty().MinimumLength(5).MaximumLength(20).Matches("[0-9]");

			RuleFor(project => project.IdResponsibleProject).NotEmpty().WithState(project => "TextBoxResponsibleProject");
			RuleFor(project => project.IdLinkedOrganization).NotEmpty().WithState(project => "TextBoxLinkedOrganization");
		}
    }
}
