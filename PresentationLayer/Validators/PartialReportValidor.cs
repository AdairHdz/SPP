using DataPersistenceLayer.Entities;
using FluentValidation;

namespace PresentationLayer.Validators
{
	public class PartialReportValidator : AbstractValidator<PartialReport>
	{
		public PartialReportValidator()
		{
			RuleFor(PartialReport => PartialReport.NumberReport).NotEmpty()
			   .MinimumLength(3).MaximumLength(30).Matches("^[a-zA-Z ]+$");
			RuleFor(PartialReport => PartialReport.HoursCovered).NotEmpty()
				.Equal(240);
			RuleFor(PartialReport => PartialReport.DeliveryDate).NotEmpty();
			RuleFor(PartialReport => PartialReport.IdProject).NotEmpty();
			RuleFor(PartialReport => PartialReport.Enrollment).NotEmpty().Matches("[a-zA-Z+]");
			RuleFor(PartialReport => PartialReport.ResultsObtained).NotEmpty().WithState(PartialReport => "TextBoxResults")
			   .MinimumLength(5).WithState(responsibleProject => "TextBoxResults")
			   .MaximumLength(500).WithState(responsibleProject => "TextBoxResults")
			   .Matches("[a-zA-Z+]").WithState(responsibleProject => "TextBoxResults");
			RuleFor(PartialReport => PartialReport.Observations).NotEmpty().WithState(PartialReport => "TextBoxObservations")
			   .MinimumLength(5).WithState(responsibleProject => "TextBoxObservations")
			   .MaximumLength(500).WithState(responsibleProject => "TextBoxObservations")
			   .Matches("[a-zA-Z+]").WithState(responsibleProject => "TextBoxObservations");
		}
	}
}
