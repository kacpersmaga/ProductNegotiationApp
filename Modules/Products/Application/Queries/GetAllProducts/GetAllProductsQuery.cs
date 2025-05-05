using MediatR;
using Products.Application.DTOs;

namespace Products.Application.Queries.GetAllProducts;

public record GetAllProductsQuery() : IRequest<List<ProductDto>>;