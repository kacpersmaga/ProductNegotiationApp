using MediatR;
namespace Negotiations.Application.Commands.RejectNegotiation;

public record RejectNegotiationCommand(Guid NegotiationId) : IRequest;