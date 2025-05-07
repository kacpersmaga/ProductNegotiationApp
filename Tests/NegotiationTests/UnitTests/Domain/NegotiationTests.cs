using Negotiations.Domain.Entities;
using Negotiations.Domain.Enums;

namespace Tests.NegotiationTests.UnitTests.Domain
{
    public class NegotiationTests
    {
        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var beforeConstruction = DateTime.UtcNow;

            // Act
            var negotiation = new Negotiation(customerId, productId);
            var afterConstruction = DateTime.UtcNow;

            // Assert
            Assert.NotEqual(Guid.Empty, negotiation.Id);
            Assert.Equal(customerId, negotiation.CustomerId);
            Assert.Equal(productId, negotiation.ProductId);
            Assert.Equal(NegotiationStatus.Pending, negotiation.Status);
            Assert.InRange(negotiation.CreatedAt, beforeConstruction, afterConstruction);
            Assert.Null(negotiation.LastRejectionDate);
            Assert.Empty(negotiation.Proposals);
            Assert.Equal(0, negotiation.RejectionCount);
        }

        [Fact]
        public void ProposeNewPrice_ShouldAddProposalWithCorrectValues()
        {
            // Arrange
            var negotiation = new Negotiation(Guid.NewGuid(), Guid.NewGuid());
            var price = 99.99m;
            var timestamp = DateTime.UtcNow;

            // Act
            negotiation.ProposeNewPrice(price, timestamp);

            // Assert
            Assert.Single(negotiation.Proposals);
            var proposal = negotiation.Proposals.First();
            Assert.Equal(negotiation.Id, proposal.NegotiationId);
            Assert.Equal(price, proposal.Price);
            Assert.Equal(timestamp, proposal.CreatedAt);
            Assert.False(proposal.Rejected);
            Assert.Equal(0, negotiation.RejectionCount);
        }

        [Fact]
        public void Reject_WhenPendingAndHasProposals_ShouldMarkLastProposalRejectedAndSetLastRejectionDate()
        {
            // Arrange
            var negotiation = new Negotiation(Guid.NewGuid(), Guid.NewGuid());
            negotiation.ProposeNewPrice(50m, DateTime.UtcNow.AddMinutes(-10));
            negotiation.ProposeNewPrice(60m, DateTime.UtcNow.AddMinutes(-5));
            var rejectionTime = DateTime.UtcNow;

            // Act
            negotiation.Reject(rejectionTime);

            // Assert
            Assert.Equal(rejectionTime, negotiation.LastRejectionDate);
            var lastProposal = negotiation.Proposals.Last();
            Assert.True(lastProposal.Rejected);
            Assert.Equal(1, negotiation.RejectionCount);
        }

        [Fact]
        public void Reject_WhenPendingAndNoProposals_ShouldOnlySetLastRejectionDate()
        {
            // Arrange
            var negotiation = new Negotiation(Guid.NewGuid(), Guid.NewGuid());
            var rejectionTime = DateTime.UtcNow;

            // Act
            negotiation.Reject(rejectionTime);

            // Assert
            Assert.Equal(rejectionTime, negotiation.LastRejectionDate);
            Assert.Empty(negotiation.Proposals);
            Assert.Equal(0, negotiation.RejectionCount);
        }

        [Theory]
        [InlineData(NegotiationStatus.Accepted)]
        [InlineData(NegotiationStatus.Cancelled)]
        public void Reject_WhenNotPending_ShouldThrowInvalidOperationException(NegotiationStatus status)
        {
            // Arrange
            var negotiation = new Negotiation(Guid.NewGuid(), Guid.NewGuid());
            // Force status
            if (status == NegotiationStatus.Accepted)
                negotiation.Accept();
            else
                negotiation.Cancel();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => negotiation.Reject(DateTime.UtcNow));
        }

        [Fact]
        public void Accept_WhenPending_ShouldChangeStatusToAccepted()
        {
            // Arrange
            var negotiation = new Negotiation(Guid.NewGuid(), Guid.NewGuid());

            // Act
            negotiation.Accept();

            // Assert
            Assert.Equal(NegotiationStatus.Accepted, negotiation.Status);
        }

        [Theory]
        [InlineData(NegotiationStatus.Accepted)]
        [InlineData(NegotiationStatus.Cancelled)]
        public void Accept_WhenNotPending_ShouldThrowInvalidOperationException(NegotiationStatus initialStatus)
        {
            // Arrange
            var negotiation = new Negotiation(Guid.NewGuid(), Guid.NewGuid());
            if (initialStatus == NegotiationStatus.Accepted)
                negotiation.Accept();
            else
                negotiation.Cancel();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => negotiation.Accept());
        }

        [Theory]
        [InlineData(NegotiationStatus.Pending)]
        [InlineData(NegotiationStatus.Accepted)]
        [InlineData(NegotiationStatus.Cancelled)]
        public void Cancel_ShouldSetStatusToCancelled(NegotiationStatus initialStatus)
        {
            // Arrange
            var negotiation = new Negotiation(Guid.NewGuid(), Guid.NewGuid());
            if (initialStatus == NegotiationStatus.Accepted)
                negotiation.Accept();
            else if (initialStatus == NegotiationStatus.Cancelled)
                negotiation.Cancel();

            // Act
            negotiation.Cancel();

            // Assert
            Assert.Equal(NegotiationStatus.Cancelled, negotiation.Status);
        }
    }
}
