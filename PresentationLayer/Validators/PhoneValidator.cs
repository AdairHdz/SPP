using DataPersistenceLayer.Entities;
using FluentValidation;

namespace PresentationLayer.Validators
{
    public class PhoneValidator : AbstractValidator<Phone>
    {
        public PhoneValidator(int tag)
        {
            RuleFor(phone => phone.Extension)
                .Matches("^[0-9]{3}$").WithState(phone => $"TextBoxPhoneExtension{tag}");
            RuleFor(phone => phone.PhoneNumber)
                .Matches("^[0-9]{10}$").WithState(phone => $"TextBoxPhoneNumber{tag}");
        }
    }
}
