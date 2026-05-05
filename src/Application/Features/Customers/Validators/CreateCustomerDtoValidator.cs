using FluentValidation;
using core_first.Application.Features.Customers.DTOs;

namespace core_first.Application.Features.Customers.Validators;

public class CreateCustomerDtoValidator : AbstractValidator<CustomerDto>
{
    public CreateCustomerDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Phone).MaximumLength(20);
        RuleFor(x => x.Email).MaximumLength(100).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
    }
}