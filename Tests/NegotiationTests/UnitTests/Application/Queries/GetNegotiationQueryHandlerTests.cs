using Microsoft.Extensions.Logging;
using Moq;
using Negotiations.Application.Queries.GetNegotiation;
using Negotiations.Domain.Entities;
using Negotiations.Domain.Interfaces;

namespace Tests.NegotiationTests.UnitTests.Application.Queries;

public class GetNegotiationQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnNegotiationDto_WhenNegotiationExists()
    {
        // Arrange
        var negotiationId = Guid.NewGuid();
        var negotiation = new Negotiation(Guid.NewGuid(), Guid.NewGuid());
        negotiation.ProposeNewPrice(100, DateTime.UtcNow);

        var repoMock = new Mock<INegotiationRepository>();
        repoMock.Setup(r => r.GetByIdAsync(negotiationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(negotiation);

        var loggerMock = new Mock<ILogger<GetNegotiationQueryHandler>>();
        var handler = new GetNegotiationQueryHandler(repoMock.Object, loggerMock.Object);

        // Act
        var result = await handler.Handle(new GetNegotiationQuery(negotiationId), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(negotiation.Id, result.Id);
        Assert.Equal(negotiation.Proposals.Count, result.Proposals.Count);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenNegotiationDoesNotExist()
    {
        // Arrange
        var negotiationId = Guid.NewGuid();

        var repoMock = new Mock<INegotiationRepository>();
        repoMock.Setup(r => r.GetByIdAsync(negotiationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Negotiation?)null);

        var loggerMock = new Mock<ILogger<GetNegotiationQueryHandler>>();
        var handler = new GetNegotiationQueryHandler(repoMock.Object, loggerMock.Object);

        // Act
        var result = await handler.Handle(new GetNegotiationQuery(negotiationId), CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}