using FluentValidation.TestHelper;
using Negotiations.Application.DTOs;
using Negotiations.Application.DTOs.Validators;

namespace Tests.NegotiationTests.UnitTests.Application.Validators;

public class ProposePriceRequestDtoValidatorTests
{
    private readonly ProposePriceRequestDtoValidator _validator = new();

    [Fact]
    public void Should_HaveError_When_NewPriceIsZeroOrNegative()
    {
        var dto = new ProposePriceRequestDto { NewPrice = 0 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.NewPrice);

        dto = new ProposePriceRequestDto { NewPrice = -5 };
        result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.NewPrice);
    }

    [Fact]
    public void Should_NotHaveError_When_NewPriceIsPositive()
    {
        var dto = new ProposePriceRequestDto { NewPrice = 99.99m };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.NewPrice);
    }
}