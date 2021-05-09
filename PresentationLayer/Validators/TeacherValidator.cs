using DataPersistenceLayer.Entities;
using FluentValidation;

namespace PresentationLayer.Validators
{
    public class TeacherValidator : AbstractValidator<Teacher>
    {
        public TeacherValidator()
        {
            RuleFor(teacher => teacher.StaffNumber)
                .NotEmpty().WithState(teacher => "TextBoxStaffNumber")
                .MaximumLength(20).WithState(teacher => "TextBoxStaffNumber");
            RuleFor(teacher => teacher.User).SetValidator(new UserValidator());
            RuleFor(teacher => teacher.User.Account).SetValidator(new AccountValidator());
        }

        public TeacherValidator(bool modify)
        {
            RuleFor(teacher => teacher.StaffNumber)
                .NotEmpty().WithState(teacher => "TextBoxStaffNumber")
                .MaximumLength(20).WithState(teacher => "TextBoxStaffNumber");
            RuleFor(teacher => teacher.User).SetValidator(new UserValidator());
        }
    }
}
