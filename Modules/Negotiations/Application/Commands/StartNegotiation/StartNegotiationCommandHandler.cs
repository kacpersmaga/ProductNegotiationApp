using MediatR;
using Microsoft.Extensions.Logging;
using Negotiations.Application.DTOs;
using Negotiations.Domain.Interfaces;

namespace Negotiations.Application.Commands.StartNegotiation;

public class StartNegotiationCommandHandler : IRequestHandler<StartNegotiationCommand, NegotiationDto>
{
    private readonly INegotiationService _service;
    private readonly ILogger<StartNegotiationCommandHandler> _logger;

    public StartNegotiationCommandHandler(INegotiationService service, ILogger<StartNegotiationCommandHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<NegotiationDto> Handle(StartNegotiationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting negotiation for customer {CustomerId} and product {ProductId}", request.CustomerId, request.ProductId);

        var negotiation = await _service.StartNegotiationAsync(request.CustomerId, request.ProductId, cancellationToken);

        return new NegotiationDto
        {
            Id = negotiation.Id,
            CustomerId = negotiation.CustomerId,
            ProductId = negotiation.ProductId,
            Status = negotiation.Status.ToString(),
            CreatedAt = negotiation.CreatedAt,
            LastRejectionDate = negotiation.LastRejectionDate,
            Proposals = new List<PriceProposalDto>()
        };
    }
}