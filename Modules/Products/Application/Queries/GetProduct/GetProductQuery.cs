using MediatR;
using Products.Application.DTOs;

namespace Products.Application.Queries.GetProduct;

public record GetProductQuery(Guid Id) : IRequest<ProductDto?>;