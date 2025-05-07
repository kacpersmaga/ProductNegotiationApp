using Negotiations.Domain.Entities;

namespace Tests.NegotiationTests.UnitTests.Domain;

public class PriceProposalTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var negotiationId = Guid.NewGuid();
        var price = 150.75m;
        var timestamp = DateTime.UtcNow;

        // Act
        var proposal = new PriceProposal(negotiationId, price, timestamp);

        // Assert
        Assert.NotEqual(Guid.Empty, proposal.Id);
        Assert.Equal(negotiationId, proposal.NegotiationId);
        Assert.Equal(price, proposal.Price);
        Assert.Equal(timestamp, proposal.CreatedAt);
        Assert.False(proposal.Rejected);
    }

    [Fact]
    public void MarkRejected_ShouldSetRejectedToTrue()
    {
        // Arrange
        var proposal = new PriceProposal(Guid.NewGuid(), 120.50m, DateTime.UtcNow);

        // Act
        proposal.MarkRejected();

        // Assert
        Assert.True(proposal.Rejected);
    }
}