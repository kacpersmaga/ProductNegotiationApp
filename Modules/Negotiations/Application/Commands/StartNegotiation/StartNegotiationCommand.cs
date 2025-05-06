using MediatR;
using Negotiations.Application.DTOs;

namespace Negotiations.Application.Commands.StartNegotiation;

public record StartNegotiationCommand(Guid CustomerId, Guid ProductId) : IRequest<NegotiationDto>;