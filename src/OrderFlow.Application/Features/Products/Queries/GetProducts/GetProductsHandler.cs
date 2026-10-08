
using FluentValidation;
using OrderFlow.Application.Abstractions.Persistence;
using OrderFlow.Application.Common.Pagination;

namespace OrderFlow.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsHandler
{
    private readonly IProductRepository _repository;
    private readonly IValidator<GetProductsQuery> _validator;

    public GetProductsHandler(IProductRepository repository, IValidator<GetProductsQuery> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<PagedResult<ProductResponse>> HandleAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(query, cancellationToken);

        var pagedProducts = await _repository.GetPagedAsync(query, cancellationToken);

        var products = pagedProducts.Items
            .Select(product => new ProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.CreatedAtUtc))
            .ToList();

        return new PagedResult<ProductResponse>(
            products,
            pagedProducts.PageNumber,
            pagedProducts.PageSize,
            pagedProducts.TotalCount);
    }
}
