
namespace OrderFlow.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? Search = null,
    decimal? MinimumPrice = null,
    decimal? MaximumPrice = null,
    string SortBy = "name",
    bool SortDescending = false);
