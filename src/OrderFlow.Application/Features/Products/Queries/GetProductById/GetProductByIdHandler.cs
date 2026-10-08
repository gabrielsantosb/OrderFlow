
using OrderFlow.Application.Abstractions.Persistence;

namespace OrderFlow.Application.Features.Products.Queries.GetProductById;

public sealed class GetProductByIdHandler
{
    private readonly IProductRepository _repository;

    public GetProductByIdHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductResponse?> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);

        if (product is null)
            return null;

        return new ProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.CreatedAtUtc);
    }
}
