namespace OrderFlow.Domain.Entities;

public sealed class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private Product()
    {
        Name = string.Empty;
        Description = string.Empty;
    }

    public Product(string name, string description, decimal price)
    {
        ValidateName(name);
        ValidatePrice(price);

        Id = Guid.NewGuid();
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Price = price;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal newPrice)
    {
        ValidatePrice(newPrice);
        Price = newPrice;
    }

    public void Rename(string newName)
    {
        ValidateName(newName);
        Name = newName.Trim();
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.", nameof(name));

        if (name.Trim().Length > 150)
            throw new ArgumentException(
                "Product name cannot exceed 150 characters.",
                nameof(name));
    }

    private static void ValidatePrice(decimal price)
    {
        if (price < 0)
            throw new ArgumentOutOfRangeException(
                nameof(price),
                "Product price cannot be negative.");
    }
}
