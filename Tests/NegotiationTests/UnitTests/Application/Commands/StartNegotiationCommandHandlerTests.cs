using Microsoft.Extensions.Logging;
using Moq;
using Negotiations.Application.Commands.StartNegotiation;
using Negotiations.Domain.Entities;
using Negotiations.Domain.Enums;
using Negotiations.Domain.Interfaces;

namespace Tests.NegotiationTests.UnitTests.Application.Commands;


public class StartNegotiationCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnCorrectDto()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var negotiation = new Negotiation(customerId, productId);
        var command = new StartNegotiationCommand(customerId, productId);

        var serviceMock = new Mock<INegotiationService>();
        serviceMock
            .Setup(s => s.StartNegotiationAsync(customerId, productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(negotiation);

        var loggerMock = new Mock<ILogger<StartNegotiationCommandHandler>>();
        var handler = new StartNegotiationCommandHandler(serviceMock.Object, loggerMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(negotiation.Id, result.Id);
        Assert.Equal(customerId, result.CustomerId);
        Assert.Equal(productId, result.ProductId);
        Assert.Equal(NegotiationStatus.Pending.ToString(), result.Status);
        Assert.Equal(negotiation.CreatedAt, result.CreatedAt);
        Assert.Null(result.LastRejectionDate);
        Assert.Empty(result.Proposals);
    }
}