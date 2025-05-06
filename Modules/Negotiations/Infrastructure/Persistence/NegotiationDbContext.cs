using Microsoft.EntityFrameworkCore;
using Negotiations.Domain.Entities;

namespace Negotiations.Infrastructure.Persistence;

public class NegotiationDbContext : DbContext
{
    public DbSet<Negotiation> Negotiations { get; set; }
    public DbSet<PriceProposal> PriceProposals { get; set; }

    public NegotiationDbContext(DbContextOptions<NegotiationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NegotiationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}