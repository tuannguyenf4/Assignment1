using api.Models;
using FluentValidation;

namespace api.Validators
{
    public class ClientValidator: AbstractValidator<Client>
    {
        public ClientValidator()
        {
            RuleFor(model => model.Id).NotNull().NotEmpty().WithMessage("Please specify a Id");
            RuleFor(model => model.FirstName).NotNull().NotEmpty().WithMessage("Please specify a First Name");
            RuleFor(model => model.LastName).NotNull().NotEmpty().WithMessage("Please specify a Last Name");
            RuleFor(model => model.PhoneNumber).NotNull().NotEmpty().WithMessage("Please specify a Phone Number");
            RuleFor(model => model.Email).NotNull().NotEmpty().WithMessage("Please specify a Email");
        }
    }
}
