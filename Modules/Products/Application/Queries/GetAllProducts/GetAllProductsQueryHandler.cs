using MediatR;
using Microsoft.Extensions.Logging;
using Products.Application.DTOs;
using Products.Domain.Interfaces;

namespace Products.Application.Queries.GetAllProducts;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _repository;
    private readonly ILogger<GetAllProductsQueryHandler> _logger;

    public GetAllProductsQueryHandler(IProductRepository repository, ILogger<GetAllProductsQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all products");

        var products = await _repository.GetAllAsync(cancellationToken);

        _logger.LogInformation("Fetched {Count} products", products.Count);

        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price
        }).ToList();
    }
}