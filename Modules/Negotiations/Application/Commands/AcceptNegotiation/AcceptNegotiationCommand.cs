using MediatR;
namespace Negotiations.Application.Commands.AcceptNegotiation;

public record AcceptNegotiationCommand(Guid NegotiationId) : IRequest;