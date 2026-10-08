
using OrderFlow.Application.Features.Products.Queries.GetProducts;
using OrderFlow.Domain.Entities;
using OrderFlow.Infrastructure.Persistence.Repositories;
using OrderFlow.IntegrationTests.Fixtures;
using Xunit;

namespace OrderFlow.IntegrationTests.Persistence;

public sealed class ProductRepositoryTests : IClassFixture<PostgreSqlFixture>
{
    private readonly PostgreSqlFixture _postgreSqlFixture;

    public ProductRepositoryTests(PostgreSqlFixture postgreSqlFixture)
    {
        _postgreSqlFixture = postgreSqlFixture;
    }

    
    [Fact]
    public async Task AddAsync_WithValidProduct_ShouldPersistProduct()
    {
        await using var databaseContext = _postgreSqlFixture.CreateDatabaseContext();

        ProductRepository productRepository = new(databaseContext);

        Product product = new(
            "Integration Test Keyboard",
            "Mechanical keyboard",
            175.50m);

        await productRepository.AddAsync(product);

        Product? savedProduct = await productRepository.GetByIdAsync(product.Id);

        Assert.NotNull(savedProduct);
        Assert.Equal(product.Id, savedProduct.Id);
        Assert.Equal(product.Name, savedProduct.Name);
        Assert.Equal(product.Price, savedProduct.Price);
    }


    [Fact]
    public async Task GetPagedAsync_WithFilters_ShouldReturnMatchingProducts()
    {
        await using var databaseContext = _postgreSqlFixture.CreateDatabaseContext();

        ProductRepository productRepository = new(databaseContext);

        string uniqueName = $"FilterTest-{Guid.NewGuid():N}";

        Product matchingProduct = new($"{uniqueName}-Keyboard", "Mechanical", 150m);
        Product expensiveProduct = new($"{uniqueName}-Mouse", "Wireless", 350m);

        await productRepository.AddAsync(matchingProduct);
        await productRepository.AddAsync(expensiveProduct);

        GetProductsQuery query = new(
            PageNumber: 1,
            PageSize: 10,
            Search: uniqueName,
            MinimumPrice: 100m,
            MaximumPrice: 200m);

        var result = await productRepository.GetPagedAsync(query);

        Assert.Single(result.Items);
        Assert.Equal(matchingProduct.Id, result.Items[0].Id);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal(1, result.TotalPages);
    }


    [Fact]
    public async Task GetPagedAsync_WithPagination_ShouldReturnCorrectPage()
    {
        await using var databaseContext = _postgreSqlFixture.CreateDatabaseContext();

        ProductRepository productRepository = new(databaseContext);

        string uniqueName = $"PaginationTest-{Guid.NewGuid():N}";

        Product firstProduct = new($"{uniqueName}-First", "Product", 100m);
        Product secondProduct = new($"{uniqueName}-Second", "Product", 200m);
        Product thirdProduct = new($"{uniqueName}-Third", "Product", 300m);

        await productRepository.AddAsync(firstProduct);
        await productRepository.AddAsync(secondProduct);
        await productRepository.AddAsync(thirdProduct);

        GetProductsQuery query = new(
            PageNumber: 2,
            PageSize: 1,
            Search: uniqueName,
            SortBy: "price");

        var result = await productRepository.GetPagedAsync(query);

        Assert.Single(result.Items);
        Assert.Equal(secondProduct.Id, result.Items[0].Id);
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(3, result.TotalPages);
        Assert.Equal(2, result.PageNumber);
    }

}
