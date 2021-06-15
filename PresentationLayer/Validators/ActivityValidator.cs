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
                .MaximumLength(255).WithState(user => "TextBoxDescription");

            RuleFor(activity => activity.ActivityType).IsInEnum();

            RuleFor(activity => activity.ValueActivity).NotEmpty().Must(ValidValue).WithState(activity => "TextBoxValue");

            RuleFor(activity => activity.ActivityStatus).IsInEnum();
        }

        public bool ValidValue(double value)
        {
            Regex regularExpression = new Regex("^[0-9]");
            string valueString = value.ToString();
            bool hasValidFormat = regularExpression.IsMatch(valueString);
            if (value > 100 || value < 1)
            {
                return false;
            }
            return hasValidFormat;
        }

        public static bool ValidDate(DateTime? dateTime)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime dateNow = Convert.ToDateTime(date);
            if (dateTime < dateNow)
            {
                return false;
            }
            return true;
        }

        public static bool ValidDateStartAndFinish(DateTime dateTimeStart, DateTime dateTimeFinish)
        {
            if (dateTimeStart > dateTimeFinish)
            {
                return false;
            }
            return true;
        }

        public static bool ValidDateFinish(DateTime? dateTimeStart, DateTime dateTimeFinish)
        {
            if (dateTimeFinish < dateTimeStart)
            {
                return false;
            }
            return true;
        }
    }
}