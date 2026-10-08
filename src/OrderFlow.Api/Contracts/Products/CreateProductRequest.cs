using System.ComponentModel.DataAnnotations;

namespace OrderFlow.Api.Contracts.Products;

public sealed class CreateProductRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
}
