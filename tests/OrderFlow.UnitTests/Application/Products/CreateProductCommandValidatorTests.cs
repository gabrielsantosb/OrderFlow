
using OrderFlow.Application.Features.Products.Commands.CreateProduct;

namespace OrderFlow.UnitTests.Application.Products;

public sealed class CreateProductCommandValidatorTests
{
    private readonly CreateProductCommandValidator _validator = new();

    [Fact]
    public async Task ValidateAsync_WithValidProduct_ShouldSucceed()
    {
        CreateProductCommand command = new(
            "Mechanical Keyboard",
            "RGB mechanical keyboard",
            125.50m);

        var validationResult = await _validator.ValidateAsync(command);

        Assert.True(validationResult.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public async Task ValidateAsync_WithEmptyName_ShouldFail(string name)
    {
        CreateProductCommand command = new(name, "Description", 100m);

        var validationResult = await _validator.ValidateAsync(command);

        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, validationFailure =>
            validationFailure.PropertyName == nameof(CreateProductCommand.Name));
    }

    [Fact]
    public async Task ValidateAsync_WithNegativePrice_ShouldFail()
    {
        CreateProductCommand command = new("Keyboard", "Description", -100m);

        var validationResult = await _validator.ValidateAsync(command);

        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, validationFailure =>
            validationFailure.PropertyName == nameof(CreateProductCommand.Price));
    }

    [Fact]
    public async Task ValidateAsync_WithNameExceedingMaximumLength_ShouldFail()
    {
        CreateProductCommand command = new(new string('A', 151), "Description", 100m);

        var validationResult = await _validator.ValidateAsync(command);

        Assert.False(validationResult.IsValid);
    }
}
