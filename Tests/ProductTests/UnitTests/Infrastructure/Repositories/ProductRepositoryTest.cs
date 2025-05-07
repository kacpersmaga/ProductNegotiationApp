using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Infrastructure.Persistence;
using Products.Infrastructure.Repositories;

namespace Tests.ProductTests.UnitTests.Infrastructure.Repositories;

public class ProductRepositoryTests
{
    private ProductDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ProductDbContext(options);
    }

    [Fact]
    public async Task AddAsync_Should_Add_Product()
    {
        var context = GetDbContext();
        var repo = new ProductRepository(context);
        var product = new Product("Test Product", 10.0m, "Test Description");

        await repo.AddAsync(product);

        var saved = await context.Products.FirstOrDefaultAsync();
        Assert.NotNull(saved);
        Assert.Equal("Test Product", saved!.Name);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Correct_Product()
    {
        var context = GetDbContext();
        var product = new Product("Test Product", 10.0m);
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var repo = new ProductRepository(context);
        var result = await repo.GetByIdAsync(product.Id);

        Assert.NotNull(result);
        Assert.Equal(product.Id, result!.Id);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Products()
    {
        var context = GetDbContext();
        context.Products.Add(new Product("A", 1.0m));
        context.Products.Add(new Product("B", 2.0m));
        await context.SaveChangesAsync();

        var repo = new ProductRepository(context);
        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count);
    }
}