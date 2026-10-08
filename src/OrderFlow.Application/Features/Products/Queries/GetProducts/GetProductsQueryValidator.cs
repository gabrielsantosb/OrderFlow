
using FluentValidation;

namespace OrderFlow.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    {
        RuleFor(query => query.PageNumber)
            .InclusiveBetween(1, 1000000);

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(query => query.Search)
            .MaximumLength(150);

        RuleFor(query => query.MinimumPrice)
            .GreaterThanOrEqualTo(0)
            .When(query => query.MinimumPrice.HasValue);

        RuleFor(query => query.MaximumPrice)
            .GreaterThanOrEqualTo(0)
            .When(query => query.MaximumPrice.HasValue);

        RuleFor(query => query.MaximumPrice)
            .GreaterThanOrEqualTo(query => query.MinimumPrice!.Value)
            .When(query => query.MinimumPrice.HasValue && query.MaximumPrice.HasValue);

        RuleFor(query => query.SortBy)
            .Must(sortBy => sortBy is "name" or "price" or "createdAt")
            .WithMessage("SortBy must be name, price or createdAt.");
    }
}
