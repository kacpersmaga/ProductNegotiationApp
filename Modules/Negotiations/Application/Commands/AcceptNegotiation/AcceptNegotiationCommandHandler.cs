using MediatR;
using Microsoft.Extensions.Logging;
using Negotiations.Domain.Interfaces;

namespace Negotiations.Application.Commands.AcceptNegotiation;

public class AcceptNegotiationCommandHandler : IRequestHandler<AcceptNegotiationCommand>
{
    private readonly INegotiationService _service;
    private readonly ILogger<AcceptNegotiationCommandHandler> _logger;

    public AcceptNegotiationCommandHandler(INegotiationService service, ILogger<AcceptNegotiationCommandHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task Handle(AcceptNegotiationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Accepting negotiation {NegotiationId}", request.NegotiationId);

        await _service.AcceptNegotiationAsync(request.NegotiationId, cancellationToken);
    }
}