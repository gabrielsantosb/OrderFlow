
using Microsoft.EntityFrameworkCore;
using OrderFlow.Application.Abstractions.Persistence;
using OrderFlow.Application.Common.Pagination;
using OrderFlow.Application.Features.Products.Queries.GetProducts;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly OrderFlowDbContext _context;

    public ProductRepository(OrderFlowDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Add(product);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(
                product => product.Id == id,
                cancellationToken);
    }

    public async Task<PagedResult<Product>> GetPagedAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<Product> productsQuery = _context.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            string searchTerm = query.Search.Trim();

            productsQuery = productsQuery.Where(product =>
                EF.Functions.ILike(product.Name, $"%{searchTerm}%"));
        }

        if (query.MinimumPrice.HasValue)
        {
            productsQuery = productsQuery.Where(product =>
                product.Price >= query.MinimumPrice.Value);
        }

        if (query.MaximumPrice.HasValue)
        {
            productsQuery = productsQuery.Where(product =>
                product.Price <= query.MaximumPrice.Value);
        }

        int totalCount = await productsQuery.CountAsync(cancellationToken);

        productsQuery = (query.SortBy, query.SortDescending) switch
        {
            ("price", false) => productsQuery.OrderBy(product => product.Price).ThenBy(product => product.Id),
            ("price", true) => productsQuery.OrderByDescending(product => product.Price).ThenBy(product => product.Id),

            ("createdAt", false) => productsQuery.OrderBy(product => product.CreatedAtUtc).ThenBy(product => product.Id),
            ("createdAt", true) => productsQuery.OrderByDescending(product => product.CreatedAtUtc).ThenBy(product => product.Id),

            (_, true) => productsQuery.OrderByDescending(product => product.Name).ThenBy(product => product.Id),
            _ => productsQuery.OrderBy(product => product.Name).ThenBy(product => product.Id)
        };

        int offset = checked((query.PageNumber - 1) * query.PageSize);

        List<Product> products = await productsQuery
            .Skip(offset)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Product>(
            products,
            query.PageNumber,
            query.PageSize,
            totalCount);
    }
}
