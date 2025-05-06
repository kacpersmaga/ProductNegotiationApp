using Negotiations.Domain.Enums;

namespace Negotiations.Domain.Entities;

public class Negotiation
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid ProductId { get; private set; }
    public NegotiationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastRejectionDate { get; private set; }
    public List<PriceProposal> Proposals { get; private set; } = new();

    public int RejectionCount => Proposals.Count(p => p.Rejected);

    private Negotiation() { }

    public Negotiation(Guid customerId, Guid productId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        ProductId = productId;
        Status = NegotiationStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void ProposeNewPrice(decimal price, DateTime timestamp)
    {
        Proposals.Add(new PriceProposal(Id, price, timestamp));
    }

    public void Reject(DateTime timestamp)
    {
        if (Status != NegotiationStatus.Pending)
            throw new InvalidOperationException("Only pending negotiations can be rejected.");

        LastRejectionDate = timestamp;
        var lastProposal = Proposals.LastOrDefault();
        if (lastProposal != null)
            lastProposal.MarkRejected();
    }

    public void Accept()
    {
        if (Status != NegotiationStatus.Pending)
            throw new InvalidOperationException("Only pending negotiations can be accepted.");

        Status = NegotiationStatus.Accepted;
    }

    public void Cancel()
    {
        Status = NegotiationStatus.Cancelled;
    }
}