using Microsoft.Extensions.Logging;
using Moq;
using Products.Application.Commands.CreateProduct;
using Products.Domain.Interfaces;

namespace Tests.ProductTests.UnitTests.Application.Commands;

public class CreateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsGuid()
    {
        var repoMock = new Mock<IProductRepository>();
        var loggerMock = new Mock<ILogger<CreateProductCommandHandler>>();
        var handler = new CreateProductCommandHandler(repoMock.Object, loggerMock.Object);
        var command = new CreateProductCommand("Test", 10m, "Test Desc");

        var result = await handler.Handle(command, CancellationToken.None);

        repoMock.Verify(r => r.AddAsync(It.IsAny<Products.Domain.Entities.Product>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotEqual(Guid.Empty, result);
    }
}