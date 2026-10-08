
using FluentValidation;

namespace OrderFlow.Application.Features.Products.Commands.CreateProduct;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(command => command.Description)
            .NotNull()
            .MaximumLength(2000);

        RuleFor(command => command.Price)
            .InclusiveBetween(0m, 9999999999999999.99m);
    }
}
