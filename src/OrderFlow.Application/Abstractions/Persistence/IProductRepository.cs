
using OrderFlow.Application.Common.Pagination;
using OrderFlow.Application.Features.Products.Queries.GetProducts;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Abstractions.Persistence;

public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken = default);

    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PagedResult<Product>> GetPagedAsync(GetProductsQuery query, CancellationToken cancellationToken = default);
}
