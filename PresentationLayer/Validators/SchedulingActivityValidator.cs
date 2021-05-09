using DataPersistenceLayer.Entities;
using FluentValidation;


namespace PresentationLayer.Validators
{
    public class SchedulingActivityValidator : AbstractValidator<SchedulingActivity>
    {
        public SchedulingActivityValidator(string nameActivity)
        {
            RuleFor(schedulingActivity => schedulingActivity.Month).NotEmpty()
            .MinimumLength(3).MaximumLength(50).Matches("[a-zA-Z]");
            RuleFor(schedulingActivity => schedulingActivity.Activity).NotEmpty().WithState(schedulingActivity => nameActivity)
            .MinimumLength(5).WithState(schedulingActivity => nameActivity)
            .MaximumLength(300).WithState(schedulingActivity => nameActivity)
            .Matches("[a-zA-Z+]").WithState(schedulingActivity => nameActivity);
        }

        public SchedulingActivityValidator()
        {
            RuleFor(schedulingActivity => schedulingActivity.Month).NotEmpty().WithState(schedulingActivity => "ComboBoxMonth")
            .MinimumLength(3).WithState(schedulingActivity => "ComboBoxMonth")
            .MaximumLength(50).WithState(schedulingActivity => "ComboBoxMonth")
            .Matches("[a-zA-Z]").WithState(schedulingActivity => "ComboBoxMonth");

            RuleFor(schedulingActivity => schedulingActivity.Activity).NotEmpty().WithState(schedulingActivity => "TextBoxActivity")
            .MinimumLength(5).WithState(schedulingActivity => "TextBoxActivity")
            .MaximumLength(300).WithState(schedulingActivity => "TextBoxActivity")
            .Matches("[a-zA-Z+]").WithState(schedulingActivity => "TextBoxActivity");
        }
    }
}
