using Negotiations.Domain.Entities;
using Negotiations.Domain.Interfaces;
using Negotiations.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Negotiations.Infrastructure.Repositories;

public class NegotiationRepository : INegotiationRepository
{
    private readonly NegotiationDbContext _dbContext;

    public NegotiationRepository(NegotiationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Negotiation?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Negotiations
            .Include(n => n.Proposals)
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }

    public async Task AddAsync(Negotiation negotiation, CancellationToken cancellationToken)
    {
        _dbContext.Negotiations.Add(negotiation);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Negotiation negotiation, CancellationToken cancellationToken)
    {
        _dbContext.Negotiations.Update(negotiation);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}