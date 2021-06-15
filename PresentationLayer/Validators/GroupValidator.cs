using DataPersistenceLayer.Entities;
using FluentValidation;

namespace PresentationLayer.Validators
{
    public class GroupValidator : AbstractValidator<Group>
    {
        public GroupValidator()
        {
            RuleFor(group => group.Term).NotEmpty().WithState(group => "ComboBoxPeriod");

            RuleFor(group => group.StaffNumber).NotEmpty().WithState(group => "LabelStaffNumber");

            RuleFor(group => group.Nrc).NotEmpty().WithState(group => "TextBoxNRC")
                .Matches("^[0-9]{5}$").WithState(group => "TextBoxNRC");
        }
    }
}
