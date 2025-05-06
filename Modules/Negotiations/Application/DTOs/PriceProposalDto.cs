namespace Negotiations.Application.DTOs;

public class PriceProposalDto
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Rejected { get; set; }
}