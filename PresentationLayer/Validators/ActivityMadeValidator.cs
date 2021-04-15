using DataPersistenceLayer.Entities;
using FluentValidation;

namespace PresentationLayer.Validators
{
    public class ActivityMadeValidator : AbstractValidator<ActivityMade>
    {
        public ActivityMadeValidator(string nameActivity, string namePlan, string nameReal)
        {
            RuleFor(ActivityMade => ActivityMade.Name).NotEmpty().WithState(PartialReport => nameActivity)
               .MinimumLength(5).WithState(PartialReport => nameActivity)
               .MaximumLength(300).WithState(PartialReport => nameActivity)
               .Matches("[a-zA-Z+]").WithState(PartialReport => nameActivity);

            RuleFor(ActivityMade => ActivityMade.PlannedWeek).NotEmpty().WithState(PartialReport => namePlan)
               .MinimumLength(3).WithState(PartialReport => namePlan)
               .MaximumLength(100).WithState(PartialReport => namePlan)
               .Matches("[a-zA-Z+]").WithState(PartialReport => namePlan);

            RuleFor(ActivityMade => ActivityMade.RealWeek).NotEmpty().WithState(PartialReport => nameReal)
               .MinimumLength(3).WithState(PartialReport => nameReal)
               .MaximumLength(100).WithState(PartialReport => nameReal)
               .Matches("[a-zA-Z+]").WithState(PartialReport => nameReal);
        }
    }
}
