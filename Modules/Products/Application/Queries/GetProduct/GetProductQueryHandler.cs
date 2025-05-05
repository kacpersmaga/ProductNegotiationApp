using MediatR;
using Microsoft.Extensions.Logging;
using Products.Application.DTOs;
using Products.Domain.Interfaces;

namespace Products.Application.Queries.GetProduct;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto?>
{
    private readonly IProductRepository _repository;
    private readonly ILogger<GetProductQueryHandler> _logger;

    public GetProductQueryHandler(IProductRepository repository, ILogger<GetProductQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching product with ID {Id}", request.Id);

        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            _logger.LogWarning("Product not found with ID {Id}", request.Id);
            return null;
        }

        _logger.LogInformation("Product found with ID {Id}", product.Id);

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        };
    }
}