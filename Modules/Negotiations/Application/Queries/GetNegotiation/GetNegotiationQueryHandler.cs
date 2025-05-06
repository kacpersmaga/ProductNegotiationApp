using MediatR;
using Microsoft.Extensions.Logging;
using Negotiations.Application.DTOs;
using Negotiations.Domain.Interfaces;

namespace Negotiations.Application.Queries.GetNegotiation;

public class GetNegotiationQueryHandler : IRequestHandler<GetNegotiationQuery, NegotiationDto?>
{
    private readonly INegotiationRepository _repository;
    private readonly ILogger<GetNegotiationQueryHandler> _logger;

    public GetNegotiationQueryHandler(INegotiationRepository repository, ILogger<GetNegotiationQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<NegotiationDto?> Handle(GetNegotiationQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching negotiation {NegotiationId}", request.NegotiationId);

        var negotiation = await _repository.GetByIdAsync(request.NegotiationId, cancellationToken);
        if (negotiation == null) return null;

        return new NegotiationDto
        {
            Id = negotiation.Id,
            CustomerId = negotiation.CustomerId,
            ProductId = negotiation.ProductId,
            Status = negotiation.Status.ToString(),
            CreatedAt = negotiation.CreatedAt,
            LastRejectionDate = negotiation.LastRejectionDate,
            Proposals = negotiation.Proposals.Select(p => new PriceProposalDto
            {
                Id = p.Id,
                Price = p.Price,
                CreatedAt = p.CreatedAt,
                Rejected = p.Rejected
            }).ToList()
        };
    }
}