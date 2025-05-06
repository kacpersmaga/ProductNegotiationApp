namespace Negotiations.Application.DTOs;

public class StartNegotiationRequestDto
{
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
}