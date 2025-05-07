using FluentValidation.TestHelper;
using Negotiations.Application.DTOs;
using Negotiations.Application.DTOs.Validators;

namespace Tests.NegotiationTests.UnitTests.Application.Validators;

public class StartNegotiationRequestDtoValidatorTests
{
    private readonly StartNegotiationRequestDtoValidator _validator = new();

    [Fact]
    public void Should_HaveError_When_CustomerIdIsEmpty()
    {
        var dto = new StartNegotiationRequestDto
        {
            CustomerId = Guid.Empty,
            ProductId = Guid.NewGuid()
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.CustomerId);
    }

    [Fact]
    public void Should_HaveError_When_ProductIdIsEmpty()
    {
        var dto = new StartNegotiationRequestDto
        {
            CustomerId = Guid.NewGuid(),
            ProductId = Guid.Empty
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
    }

    [Fact]
    public void Should_NotHaveError_When_BothIdsAreValid()
    {
        var dto = new StartNegotiationRequestDto
        {
            CustomerId = Guid.NewGuid(),
            ProductId = Guid.NewGuid()
        };

        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.CustomerId);
        result.ShouldNotHaveValidationErrorFor(x => x.ProductId);
    }
}