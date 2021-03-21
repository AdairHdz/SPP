using DataPersistenceLayer.Entities;
using FluentValidation;

namespace PresentationLayer.Validators
{
    class PhoneValidator : AbstractValidator<Phone>
    {
        public PhoneValidator()
        {
            RuleFor(phone => phone.Extension).NotEmpty().Matches("[0-9+]")
                .WithState(phone => "TextBoxExtesion");
            RuleFor(phone => phone.PhoneNumber).NotEmpty().Matches("[0-9+]")
                .WithState(phone => "TextBoxPhoneNumber");
        }
    }
}