using Microsoft.Extensions.Logging;
using Moq;
using Negotiations.Application.Commands.RejectNegotiation;
using Negotiations.Domain.Interfaces;

namespace Tests.NegotiationTests.UnitTests.Application.Commands;

public class RejectNegotiationCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_CallServiceWithCorrectId()
    {
        // Arrange
        var negotiationId = Guid.NewGuid();
        var command = new RejectNegotiationCommand(negotiationId);
        var serviceMock = new Mock<INegotiationService>();
        var loggerMock = new Mock<ILogger<RejectNegotiationCommandHandler>>();
        var handler = new RejectNegotiationCommandHandler(serviceMock.Object, loggerMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        serviceMock.Verify(s => s.RejectNegotiationAsync(negotiationId, It.IsAny<CancellationToken>()), Times.Once);
    }
}