
using FluentValidation;
using NSubstitute;
using OrderFlow.Application.Abstractions.Persistence;
using OrderFlow.Application.Features.Products.Commands.CreateProduct;
using OrderFlow.Domain.Entities;

namespace OrderFlow.UnitTests.Application.Products;

public sealed class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();

        IValidator<CreateProductCommand> validator = new CreateProductCommandValidator();

        _handler = new CreateProductHandler(_productRepository, validator);
    }

    [Fact]
    public async Task HandleAsync_WithValidCommand_ShouldSaveProduct()
    {
        CreateProductCommand command = new("Keyboard", "Mechanical keyboard", 150m);

        Guid productId = await _handler.HandleAsync(command);

        Assert.NotEqual(Guid.Empty, productId);

        await _productRepository.Received(1).AddAsync(
            Arg.Is<Product>(product =>
                product.Id == productId &&
                product.Name == command.Name &&
                product.Price == command.Price),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WithInvalidCommand_ShouldNotSaveProduct()
    {
        CreateProductCommand command = new("", "Description", -10m);

        await Assert.ThrowsAsync<ValidationException>(
            () => _handler.HandleAsync(command));

        await _productRepository.DidNotReceive().AddAsync(
            Arg.Any<Product>(),
            Arg.Any<CancellationToken>());
    }
}
