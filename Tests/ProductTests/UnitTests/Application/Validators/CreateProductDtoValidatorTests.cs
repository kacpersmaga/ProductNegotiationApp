using FluentValidation.TestHelper;
using Products.Application.DTOs;
using Products.Application.DTOs.Validators;

namespace Tests.ProductTests.UnitTests.Application.Validators;

public class CreateProductDtoValidatorTests
{
    private readonly CreateProductDtoValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new CreateProductDto { Name = "", Price = 10 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Too_Long()
    {
        var model = new CreateProductDto
        {
            Name = "Valid Name",
            Price = 10,
            Description = new string('a', 501)
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Price_Is_Negative()
    {
        var model = new CreateProductDto { Name = "Valid", Price = -5 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Should_Not_Have_Errors_For_Valid_Model()
    {
        var model = new CreateProductDto { Name = "Valid", Price = 99.99m, Description = "Test" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}