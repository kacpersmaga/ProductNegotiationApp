using Microsoft.Extensions.Logging;
using Moq;
using Products.Application.Queries.GetProduct;
using Products.Domain.Entities;
using Products.Domain.Interfaces;

namespace Tests.ProductTests.UnitTests.Application.Queries;

public class GetProductQueryHandlerTests
{
    [Fact]
    public async Task Handle_ProductExists_ReturnsProductDto()
    {
        var product = new Product("Item", 5.5m);
        var repoMock = new Mock<IProductRepository>();
        repoMock.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<CancellationToken>())).ReturnsAsync(product);

        var loggerMock = new Mock<ILogger<GetProductQueryHandler>>();
        var handler = new GetProductQueryHandler(repoMock.Object, loggerMock.Object);

        var result = await handler.Handle(new GetProductQuery(product.Id), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(product.Name, result!.Name);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ReturnsNull()
    {
        var repoMock = new Mock<IProductRepository>();
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

        var loggerMock = new Mock<ILogger<GetProductQueryHandler>>();
        var handler = new GetProductQueryHandler(repoMock.Object, loggerMock.Object);

        var result = await handler.Handle(new GetProductQuery(Guid.NewGuid()), CancellationToken.None);

        Assert.Null(result);
    }
}