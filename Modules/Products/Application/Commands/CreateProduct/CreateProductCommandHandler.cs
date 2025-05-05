using MediatR;
using Microsoft.Extensions.Logging;
using Products.Domain.Entities;
using Products.Domain.Interfaces;

namespace Products.Application.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _repository;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(IProductRepository repository, ILogger<CreateProductCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating product {@Request}", request);

        var product = new Product(request.Name, request.Price, request.Description);
        await _repository.AddAsync(product, cancellationToken);

        _logger.LogInformation("Product created successfully with ID {ProductId}", product.Id);
        return product.Id;
    }
}