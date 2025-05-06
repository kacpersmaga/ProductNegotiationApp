namespace Negotiations.Domain.Entities;

public class PriceProposal
{
    public Guid Id { get; private set; }
    public Guid NegotiationId { get; private set; }
    public decimal Price { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool Rejected { get; private set; }

    private PriceProposal() { }

    public PriceProposal(Guid negotiationId, decimal price, DateTime timestamp)
    {
        Id = Guid.NewGuid();
        NegotiationId = negotiationId;
        Price = price;
        CreatedAt = timestamp;
    }

    public void MarkRejected()
    {
        Rejected = true;
    }
}