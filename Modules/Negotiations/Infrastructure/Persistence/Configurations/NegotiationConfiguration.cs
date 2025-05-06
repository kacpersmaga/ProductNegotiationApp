using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Negotiations.Domain.Entities;

namespace Negotiations.Infrastructure.Persistence.Configurations;

public class NegotiationConfiguration : IEntityTypeConfiguration<Negotiation>
{
    public void Configure(EntityTypeBuilder<Negotiation> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.CustomerId)
            .IsRequired();

        builder.Property(n => n.ProductId)
            .IsRequired();

        builder.Property(n => n.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(n => n.CreatedAt)
            .IsRequired();

        builder.Property(n => n.LastRejectionDate);

        builder.HasMany(n => n.Proposals)
            .WithOne()
            .HasForeignKey(p => p.NegotiationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}