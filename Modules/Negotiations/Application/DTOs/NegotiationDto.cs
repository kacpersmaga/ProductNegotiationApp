namespace Negotiations.Application.DTOs;

public class NegotiationDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastRejectionDate { get; set; }
    public List<PriceProposalDto> Proposals { get; set; } = new();
}