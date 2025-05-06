using MediatR;
using Negotiations.Application.DTOs;

namespace Negotiations.Application.Queries.GetNegotiation;

public record GetNegotiationQuery(Guid NegotiationId) : IRequest<NegotiationDto?>;