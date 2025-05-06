using FluentValidation;

namespace Negotiations.Application.DTOs.Validators
{
    public class StartNegotiationRequestDtoValidator : AbstractValidator<StartNegotiationRequestDto>
    {
        public StartNegotiationRequestDtoValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");
        }
    }
}