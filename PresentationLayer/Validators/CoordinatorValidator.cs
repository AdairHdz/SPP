using DataPersistenceLayer.Entities;
using FluentValidation;

namespace PresentationLayer.Validators
{
    public class CoordinatorValidator : AbstractValidator<Coordinator>
    {
        public CoordinatorValidator()
        {
            RuleFor(coordinator => coordinator.StaffNumber)
                .NotEmpty().WithState(coordinator => "TextBoxStaffNumber")
                .MaximumLength(20).WithState(coordinator => "TextBoxStaffNumber");
            RuleFor(coordinator => coordinator.User).SetValidator(new UserValidator());
            RuleFor(coordinator => coordinator.User.Account).SetValidator(new AccountValidator());
        }
    }
}
