using MediatR;
using Microsoft.Extensions.Logging;
using Negotiations.Domain.Interfaces;

namespace Negotiations.Application.Commands.ProposePrice;

public class ProposePriceCommandHandler : IRequestHandler<ProposePriceCommand>
{
    private readonly INegotiationService _service;
    private readonly ILogger<ProposePriceCommandHandler> _logger;

    public ProposePriceCommandHandler(INegotiationService service, ILogger<ProposePriceCommandHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task Handle(ProposePriceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Client proposes new price {Price} for negotiation {NegotiationId}", request.NewPrice, request.NegotiationId);

        await _service.ProposePriceAsync(request.NegotiationId, request.NewPrice, cancellationToken);
    }
}