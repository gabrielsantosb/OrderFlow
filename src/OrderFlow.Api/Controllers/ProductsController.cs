using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Api.Contracts.Products;
using OrderFlow.Application.Common.Pagination;
using OrderFlow.Application.Features.Products;
using OrderFlow.Application.Features.Products.Commands.CreateProduct;
using OrderFlow.Application.Features.Products.Queries.GetProductById;
using OrderFlow.Application.Features.Products.Queries.GetProducts;

namespace OrderFlow.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public sealed class ProductsController : ControllerBase
    {
        private readonly CreateProductHandler _createHandler;
        private readonly GetProductByIdHandler _getByIdHandler;
        private readonly GetProductsHandler _getAllHandler;

        public ProductsController(CreateProductHandler createHandler, GetProductByIdHandler getByIdHandler, GetProductsHandler getAllHandler)
        {
            _createHandler = createHandler;
            _getByIdHandler = getByIdHandler;
            _getAllHandler = getAllHandler;
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponse>> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateProductCommand(
                request.Name,
                request.Description,
                request.Price);

            var id = await _createHandler.HandleAsync(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new { id });
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var product = await _getByIdHandler.HandleAsync(id, cancellationToken);

            if (product is null)
                return NotFound();

            return Ok(product);
        }

        
        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductResponse>>> GetAll([FromQuery] GetProductsQuery query, CancellationToken cancellationToken)
        {
            var products = await _getAllHandler.HandleAsync(query, cancellationToken);

            return Ok(products);
        }

    }
}
