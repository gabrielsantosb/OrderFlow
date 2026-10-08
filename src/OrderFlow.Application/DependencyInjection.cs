using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderFlow.Application.Features.Products.Commands.CreateProduct;
using OrderFlow.Application.Features.Products.Queries.GetProductById;
using OrderFlow.Application.Features.Products.Queries.GetProducts;

namespace OrderFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();

        services.AddScoped<GetProductsHandler>();
        services.AddScoped<GetProductByIdHandler>();
        services.AddScoped<CreateProductHandler>();

        return services;
    }
}
