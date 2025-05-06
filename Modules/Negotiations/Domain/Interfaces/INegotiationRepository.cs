using Negotiations.Domain.Entities;

namespace Negotiations.Domain.Interfaces;

public interface INegotiationRepository
{
    Task<Negotiation?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Negotiation negotiation, CancellationToken cancellationToken);
    Task UpdateAsync(Negotiation negotiation, CancellationToken cancellationToken);
}