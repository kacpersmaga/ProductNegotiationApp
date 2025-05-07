using Microsoft.Extensions.Logging;
using Moq;
using Negotiations.Application.Commands.AcceptNegotiation;
using Negotiations.Domain.Interfaces;

namespace Tests.NegotiationTests.UnitTests.Application.Commands;

public class AcceptNegotiationCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_CallServiceWithCorrectId()
    {
        // Arrange
        var negotiationId = Guid.NewGuid();
        var command = new AcceptNegotiationCommand(negotiationId);
        var serviceMock = new Mock<INegotiationService>();
        var loggerMock = new Mock<ILogger<AcceptNegotiationCommandHandler>>();
        var handler = new AcceptNegotiationCommandHandler(serviceMock.Object, loggerMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        serviceMock.Verify(s => s.AcceptNegotiationAsync(negotiationId, It.IsAny<CancellationToken>()), Times.Once);
    }
}