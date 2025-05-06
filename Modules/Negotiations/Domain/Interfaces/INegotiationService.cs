using Negotiations.Domain.Entities;

namespace Negotiations.Domain.Interfaces;

public interface INegotiationService
{
    Task<Negotiation> StartNegotiationAsync(Guid customerId, Guid productId, CancellationToken cancellationToken);
    Task ProposePriceAsync(Guid negotiationId, decimal newPrice, CancellationToken cancellationToken);
    Task AcceptNegotiationAsync(Guid negotiationId, CancellationToken cancellationToken);
    Task RejectNegotiationAsync(Guid negotiationId, CancellationToken cancellationToken);
}