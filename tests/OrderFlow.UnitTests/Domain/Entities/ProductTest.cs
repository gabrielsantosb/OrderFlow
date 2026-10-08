using OrderFlow.Domain.Entities;

namespace OrderFlow.UnitTests.Domain.Entities;

public class ProductTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateProduct()
    {
        //Arrange
        const string name = "Gaming Keyboard";
        const string description = "Mechanical keyboard";
        const decimal price = 150.00m;

        //Act
        var product = new Product(name, description, price);

        //Assert
        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.Equal(name, product.Name);
        Assert.Equal(description, product.Description);
        Assert.Equal(price, product.Price);
    }

    [Fact]
    public void Constructor_WithNegativePrice_ShouldThrowException()
    {
        // Act
        var action = () => new Product("Keyboard", "Mechanical", -100);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void Constructor_WithInvalidName_ShouldThrowException(string name)
    {
        // Act
        var action = () => new Product(name, "Description", 100);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void UpdatePrice_WithValidPrice_ShouldUpdatePrice()
    {
        // Arrange
        var product = new Product("Mouse", "Wireless", 50);

        // Act
        product.UpdatePrice(75);

        // Assert
        Assert.Equal(75, product.Price);
    }

    [Fact]
    public void UpdatePrice_WithNegativePrice_ShouldThrowException()
    {
        // Arrange
        var product = new Product("Mouse", "Wireless", 50);

        // Act
        var action = () => product.UpdatePrice(-10);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
        Assert.Equal(50, product.Price);
    }
}
