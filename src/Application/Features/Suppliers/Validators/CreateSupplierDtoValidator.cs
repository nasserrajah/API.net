using FluentValidation;
using core_first.Application.Features.Suppliers.DTOs;

namespace core_first.Application.Features.Suppliers.Validators;

public class CreateSupplierDtoValidator : AbstractValidator<SupplierDto>
{
    public CreateSupplierDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Phone).MaximumLength(20);
        RuleFor(x => x.Email).MaximumLength(100).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
    }
}