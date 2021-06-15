using DataPersistenceLayer.Entities;
using FluentValidation;
using System;

namespace PresentationLayer.Validators
{
	public class MonthlyReportValidator : AbstractValidator<MonthlyReport>
	{
		public MonthlyReportValidator()
		{
			RuleFor(MonthlyReport => MonthlyReport.PerformedActivities).NotEmpty().WithState(monthlyReport => "TextBoxActivities")
			   .MinimumLength(3).MaximumLength(65000).WithState(monthlyReport => "TextBoxActivities");
			RuleFor(MonthlyReport => MonthlyReport.ResultsObtained).NotEmpty().WithState(monthlyReport => "TextBoxResults")
			   .MinimumLength(3).MaximumLength(65000).WithState(monthlyReport => "TextBoxResults");
			RuleFor(MonthlyReport => MonthlyReport.HoursReported).NotEmpty().WithState(monthlyReport => "TextBoxReportedHours");

		}

		public static bool ValidDate(DateTime? dateSend)
        {
			DateTime dateTime = Convert.ToDateTime(dateSend);
			int year = dateTime.Year;
			int yearNow = DateTime.Now.Year;
			if (year == yearNow)
			{
				return true;
			}
			return false;
		}
	}
}
