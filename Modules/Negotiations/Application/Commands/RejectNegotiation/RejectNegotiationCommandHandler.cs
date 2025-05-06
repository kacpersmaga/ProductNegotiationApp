using MediatR;
using Microsoft.Extensions.Logging;
using Negotiations.Domain.Interfaces;

namespace Negotiations.Application.Commands.RejectNegotiation;

public class RejectNegotiationCommandHandler : IRequestHandler<RejectNegotiationCommand>
{
    private readonly INegotiationService _service;
    private readonly ILogger<RejectNegotiationCommandHandler> _logger;

    public RejectNegotiationCommandHandler(INegotiationService service, ILogger<RejectNegotiationCommandHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task Handle(RejectNegotiationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Rejecting negotiation {NegotiationId}", request.NegotiationId);

        await _service.RejectNegotiationAsync(request.NegotiationId, cancellationToken);
    }
}