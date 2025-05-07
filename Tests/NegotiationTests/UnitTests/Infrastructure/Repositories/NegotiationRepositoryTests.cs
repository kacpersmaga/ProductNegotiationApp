using Microsoft.EntityFrameworkCore;
using Negotiations.Domain.Entities;
using Negotiations.Infrastructure.Persistence;
using Negotiations.Infrastructure.Repositories;

namespace Tests.NegotiationTests.UnitTests.Infrastructure.Repositories;

public class NegotiationRepositoryTests
{
    private NegotiationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<NegotiationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new NegotiationDbContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldAddNegotiation()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var repo = new NegotiationRepository(context);
        var negotiation = new Negotiation(Guid.NewGuid(), Guid.NewGuid());

        // Act
        await repo.AddAsync(negotiation, CancellationToken.None);
        var stored = await context.Negotiations.FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(stored);
        Assert.Equal(negotiation.Id, stored.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNegotiationWithProposals()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var negotiation = new Negotiation(Guid.NewGuid(), Guid.NewGuid());
        negotiation.ProposeNewPrice(99, DateTime.UtcNow);

        context.Negotiations.Add(negotiation);
        await context.SaveChangesAsync();

        var repo = new NegotiationRepository(context);

        // Act
        var result = await repo.GetByIdAsync(negotiation.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Proposals);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistNewProposals()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var negotiation = new Negotiation(Guid.NewGuid(), Guid.NewGuid());
        context.Negotiations.Add(negotiation);
        await context.SaveChangesAsync();

        var repo = new NegotiationRepository(context);
        negotiation.ProposeNewPrice(199, DateTime.UtcNow);

        // Act
        await repo.UpdateAsync(negotiation, CancellationToken.None);
        var result = await context.Negotiations.Include(n => n.Proposals).FirstOrDefaultAsync(n => n.Id == negotiation.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Proposals);
    }
}