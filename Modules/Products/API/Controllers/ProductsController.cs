using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Products.Application.Commands.CreateProduct;
using Products.Application.DTOs;
using Products.Application.Queries.GetAllProducts;
using Products.Application.Queries.GetProduct;
using Shared.API.Common;

namespace Products.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Guid>>> Create(
        [FromBody] CreateProductDto dto,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to create product");
        
        var command = new CreateProductCommand(dto.Name, dto.Price, dto.Description);
        var id = await _mediator.Send(command, cancellationToken);
        return Ok(ApiResponse<Guid>.Ok(id));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to fetch product with ID {Id}", id);

        var product = await _mediator.Send(new GetProductQuery(id), cancellationToken);

        if (product is null)
        {
            _logger.LogWarning("Product not found with ID {Id}", id);
            return NotFound(ApiResponse<ProductDto>.Fail("Product not found"));
        }

        return Ok(ApiResponse<ProductDto>.Ok(product));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ProductDto>>>> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to fetch all products");

        var products = await _mediator.Send(new GetAllProductsQuery(), cancellationToken);
        return Ok(ApiResponse<List<ProductDto>>.Ok(products));
    }
}