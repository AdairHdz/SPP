using DataPersistenceLayer.Entities;
using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace PresentationLayer.Validators
{
    public class ActivityValidator : AbstractValidator<Activity>
    {
        public ActivityValidator()
        {
            RuleFor(activity => activity.Name).NotEmpty().WithState(user => "TextBoxName")
                .MaximumLength(150).WithState(user => "TextBoxName");

            RuleFor(activity => activity.Description).NotEmpty().WithState(user => "TextBoxDescription")
                .MaximumLength(150).WithState(user => "TextBoxDescription");

            RuleFor(activity => activity.ActivityType).IsInEnum();

            RuleFor(activity => activity.ValueActivity).NotEmpty().Must(ValidValue).WithState(activity => "TextBoxValue");

            RuleFor(activity => activity.ActivityStatus).IsInEnum();

            RuleFor(activity => activity.StartDate).NotEmpty().Must(ValidStartDate);

            RuleFor(activity => activity.FinishDate).NotEmpty().Must(ValidStartDate);
        }

        public bool ValidValue(double value)
        {
            Regex regularExpression = new Regex("^[0-9]{2}$");
            string valueString = value.ToString();
            bool hasValidFormat = regularExpression.IsMatch(valueString);
            if (value > 100 || value < 1)
            {
                return false;
            }
            return hasValidFormat;
        }

        public bool ValidStartDate(DateTime? dateTime)
        {
            DateTime dateNow = DateTime.UtcNow;
            if (dateTime < dateNow)
            {
                return false;
            }
            return true;
        }
    }
}