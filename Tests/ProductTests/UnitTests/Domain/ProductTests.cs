using Products.Domain.Entities;

namespace Tests.ProductTests.UnitTests.Domain;

public class ProductTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var product = new Product("Test", 9.99m, "Test description");

        Assert.Equal("Test", product.Name);
        Assert.Equal(9.99m, product.Price);
        Assert.Equal("Test description", product.Description);
        Assert.NotEqual(Guid.Empty, product.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void SetName_Invalid_Throws(string name)
    {
        var product = new Product("Valid", 1);
        Assert.Throws<ArgumentException>(() => product.SetName(name));
    }

    [Fact]
    public void SetPrice_Negative_Throws()
    {
        var product = new Product("Valid", 1);
        Assert.Throws<ArgumentOutOfRangeException>(() => product.SetPrice(-5));
    }
}