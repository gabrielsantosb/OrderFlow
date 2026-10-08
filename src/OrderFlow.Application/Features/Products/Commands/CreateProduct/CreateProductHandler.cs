
using FluentValidation;
using OrderFlow.Application.Abstractions.Persistence;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Features.Products.Commands.CreateProduct;

public sealed class CreateProductHandler
{
    private readonly IProductRepository _repository;
    private readonly IValidator<CreateProductCommand> _validator;

    public CreateProductHandler(IProductRepository repository, IValidator<CreateProductCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Guid> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        Product product = new Product(
            command.Name,
            command.Description,
            command.Price);

        await _repository.AddAsync(product, cancellationToken);

        return product.Id;
    }
}
