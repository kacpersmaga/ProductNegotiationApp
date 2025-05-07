using Microsoft.Extensions.Logging;
using Moq;
using Products.Application.Queries.GetAllProducts;
using Products.Domain.Entities;
using Products.Domain.Interfaces;

namespace Tests.ProductTests.UnitTests.Application.Queries;

public class GetAllProductsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsMappedProducts()
    {
        var products = new List<Product> { new("Product1", 1.0m) };
        var repoMock = new Mock<IProductRepository>();
        repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(products);
        var loggerMock = new Mock<ILogger<GetAllProductsQueryHandler>>();

        var handler = new GetAllProductsQueryHandler(repoMock.Object, loggerMock.Object);

        var result = await handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        Assert.Single(result);
        Assert.Equal("Product1", result[0].Name);
    }
}