using EShop.Application.Features.Products.Commands;
using FluentValidation;

namespace EShop.Application.Features.Products.Validators;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage("Məhsul adı boş ola bilməz.")
            .MinimumLength(2)
                .WithMessage("Məhsul adının ölçüsü 2 və ya daha artıq olmalıdır.")
            .MaximumLength(50)
                .WithMessage("Məhsul adının ölçüsü 50 və ya daha az olmalıdır.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
                .WithMessage("Məhsul miqdarı pozitiv olmalıdır.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
                .WithMessage("Məhsul qiyməti pozitiv olmalıdır.");
    }
}
