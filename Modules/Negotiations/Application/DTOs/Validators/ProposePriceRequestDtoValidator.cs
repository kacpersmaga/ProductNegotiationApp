using FluentValidation;
using Negotiations.Application.DTOs;

namespace Negotiations.Application.DTOs.Validators
{
    public class ProposePriceRequestDtoValidator : AbstractValidator<ProposePriceRequestDto>
    {
        public ProposePriceRequestDtoValidator()
        {
            RuleFor(x => x.NewPrice)
                .GreaterThan(0)
                .WithMessage("Proposed price must be greater than 0.");
        }
    }
}