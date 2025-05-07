using Microsoft.Extensions.Logging;
using Moq;
using Negotiations.Application.Commands.ProposePrice;
using Negotiations.Domain.Interfaces;

namespace Tests.NegotiationTests.UnitTests.Application.Commands;

public class ProposePriceCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_CallServiceWithCorrectData()
    {
        // Arrange
        var negotiationId = Guid.NewGuid();
        var price = 250.50m;
        var command = new ProposePriceCommand(negotiationId, price);
        var serviceMock = new Mock<INegotiationService>();
        var loggerMock = new Mock<ILogger<ProposePriceCommandHandler>>();
        var handler = new ProposePriceCommandHandler(serviceMock.Object, loggerMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        serviceMock.Verify(s => s.ProposePriceAsync(negotiationId, price, It.IsAny<CancellationToken>()), Times.Once);
    }
}