namespace Products.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }

    private Product() { }

    public Product(string name, decimal price, string? description = null)
    {
        SetName(name);
        SetPrice(price);
        SetDescription(description);
        Id = Guid.NewGuid();
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.", nameof(name));
        
        if (name.Length > 100)
            throw new ArgumentException("Product name cannot exceed 100 characters.", nameof(name));

        Name = name;
    }

    public void SetDescription(string? description)
    {
        if (description?.Length > 500)
            throw new ArgumentException("Description is too long.", nameof(description));

        Description = description;
    }

    public void SetPrice(decimal price)
    {
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be non-negative.");

        Price = price;
    }
}