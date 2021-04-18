using DataPersistenceLayer.Entities;
using FluentValidation;

namespace PresentationLayer.Validators
{
    public class ActivityMadeValidator : AbstractValidator<ActivityMade>
    {
        public ActivityMadeValidator()
        {
            RuleFor(ActivityMade => ActivityMade.Name).NotEmpty().WithState(PartialReport => "FormGridWeek")
               .MinimumLength(5).WithState(PartialReport => "FormGridWeek")
               .MaximumLength(300).WithState(PartialReport => "FormGridWeek")
               .Matches("[a-zA-Z+]").WithState(PartialReport => "FormGridWeek");
        }
    }
}
