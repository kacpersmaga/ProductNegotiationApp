using Negotiations.Domain.Entities;
using Negotiations.Domain.Interfaces;
using Shared.Application.Interfaces;

namespace Negotiations.Application.Services;

public class NegotiationService : INegotiationService
{
    private readonly INegotiationRepository _repository;
    private readonly IDateTimeService _dateTime;

    public NegotiationService(INegotiationRepository repository, IDateTimeService dateTime)
    {
        _repository = repository;
        _dateTime = dateTime;
    }

    public async Task<Negotiation> StartNegotiationAsync(Guid customerId, Guid productId, CancellationToken cancellationToken)
    {
        var negotiation = new Negotiation(customerId, productId);
        await _repository.AddAsync(negotiation, cancellationToken);
        return negotiation;
    }

    public async Task ProposePriceAsync(Guid negotiationId, decimal newPrice, CancellationToken cancellationToken)
    {
        var negotiation = await _repository.GetByIdAsync(negotiationId, cancellationToken)
            ?? throw new InvalidOperationException("Negotiation not found.");

        if (negotiation.RejectionCount >= 3)
            throw new InvalidOperationException("Maximum number of proposals reached.");

        if (negotiation.LastRejectionDate != null &&
            _dateTime.UtcNow > negotiation.LastRejectionDate.Value.AddDays(7))
        {
            negotiation.Cancel();
            await _repository.UpdateAsync(negotiation, cancellationToken);
            return;
        }

        negotiation.ProposeNewPrice(newPrice, _dateTime.UtcNow);
        await _repository.UpdateAsync(negotiation, cancellationToken);
    }

    public async Task AcceptNegotiationAsync(Guid negotiationId, CancellationToken cancellationToken)
    {
        var negotiation = await _repository.GetByIdAsync(negotiationId, cancellationToken)
            ?? throw new InvalidOperationException("Negotiation not found.");

        negotiation.Accept();
        await _repository.UpdateAsync(negotiation, cancellationToken);
    }

    public async Task RejectNegotiationAsync(Guid negotiationId, CancellationToken cancellationToken)
    {
        var negotiation = await _repository.GetByIdAsync(negotiationId, cancellationToken)
            ?? throw new InvalidOperationException("Negotiation not found.");

        negotiation.Reject(_dateTime.UtcNow);
        await _repository.UpdateAsync(negotiation, cancellationToken);
    }
}