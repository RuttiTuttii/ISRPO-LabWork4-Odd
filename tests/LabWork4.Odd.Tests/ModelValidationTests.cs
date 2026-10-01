using LabWork4.Odd.Models;
using Xunit;

namespace LabWork4.Odd.Tests;

public class ModelValidationTests
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Category_ThrowsArgumentException_WhenNameIsEmpty(string? invalidName)
    {
        Assert.Throws<ArgumentException>(() => new Category { Name = invalidName! });
    }

    [Fact]
    public void Category_TrimsName_WhenValid()
    {
        var category = new Category { Name = "  Электроника  " };
        Assert.Equal("Электроника", category.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Product_ThrowsArgumentException_WhenNameIsEmpty(string? invalidName)
    {
        Assert.Throws<ArgumentException>(() => new Product { Name = invalidName! });
    }

    [Fact]
    public void Product_ThrowsArgumentException_WhenPriceIsNegative()
    {
        Assert.Throws<ArgumentException>(() => new Product
        {
            Name = "Тестовый товар",
            Price = -50m
        });
    }

    [Fact]
    public void Product_AcceptsZeroOrPositivePrice()
    {
        var product = new Product { Name = "Бесплатный образец", Price = 0m };
        Assert.Equal(0m, product.Price);

        product.Price = 199.99m;
        Assert.Equal(199.99m, product.Price);
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid-email")]
    [InlineData("test.com")]
    public void Customer_ThrowsArgumentException_WhenEmailIsInvalid(string invalidEmail)
    {
        Assert.Throws<ArgumentException>(() => new Customer
        {
            Name = "Иван",
            Email = invalidEmail
        });
    }

    [Fact]
    public void Order_CalculatesTotalAmount_Correctly()
    {
        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new() { Quantity = 2, UnitPrice = 100m },
                new() { Quantity = 3, UnitPrice = 50m }
            }
        };

        Assert.Equal(350m, order.TotalAmount);
    }
}
