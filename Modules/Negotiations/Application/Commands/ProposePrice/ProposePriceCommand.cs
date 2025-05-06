using MediatR;
namespace Negotiations.Application.Commands.ProposePrice;

public record ProposePriceCommand(Guid NegotiationId, decimal NewPrice) : IRequest;