using LabWork4.Odd.Data;
using LabWork4.Odd.Filtering;
using LabWork4.Odd.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LabWork4.Odd.Tests;

public class FilteringAndPaginationTests
{
    private AppDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        var cat1 = new Category { Id = 1, Name = "Книги" };
        var cat2 = new Category { Id = 2, Name = "Гаджеты" };
        context.Categories.AddRange(cat1, cat2);

        context.Products.AddRange(
            new Product { Id = 1, Name = "C# в подлиннике", Price = 1500m, StockQuantity = 10, CategoryId = 1 },
            new Product { Id = 2, Name = "CLR via C#", Price = 3500m, StockQuantity = 0, CategoryId = 1 },
            new Product { Id = 3, Name = "Паттерны проектирования GoF", Price = 2500m, StockQuantity = 5, CategoryId = 1 },
            new Product { Id = 4, Name = "Смарт-часы Pro", Price = 12000m, StockQuantity = 8, CategoryId = 2 },
            new Product { Id = 5, Name = "Фитнес-браслет Lite", Price = 2500m, StockQuantity = 15, CategoryId = 2 }
        );

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task ToPagedResultAsync_ReturnsCorrectPageCount_AndHasNextFlag()
    {
        using var db = CreateInMemoryDbContext();

        var resultPage1 = await db.Products.OrderBy(p => p.Id).ToPagedResultAsync(pageNumber: 1, pageSize: 2);
        Assert.Equal(5, resultPage1.TotalCount);
        Assert.Equal(3, resultPage1.TotalPages);
        Assert.Equal(2, resultPage1.Items.Count);
        Assert.False(resultPage1.HasPreviousPage);
        Assert.True(resultPage1.HasNextPage);

        var resultPage3 = await db.Products.OrderBy(p => p.Id).ToPagedResultAsync(pageNumber: 3, pageSize: 2);
        Assert.Single(resultPage3.Items);
        Assert.True(resultPage3.HasPreviousPage);
        Assert.False(resultPage3.HasNextPage);
    }

    [Fact]
    public void ApplyFilter_FiltersBySearchTerm_CaseInsensitively()
    {
        using var db = CreateInMemoryDbContext();

        var filter = new ProductFilter { SearchTerm = "c#" };
        var filtered = db.Products.ApplyFilter(filter).ToList();

        Assert.Equal(2, filtered.Count);
        Assert.All(filtered, p => Assert.Contains("C#", p.Name, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ApplyFilter_FiltersByPriceRange()
    {
        using var db = CreateInMemoryDbContext();

        var filter = new ProductFilter { MinPrice = 2000m, MaxPrice = 3000m };
        var filtered = db.Products.ApplyFilter(filter).ToList();

        Assert.Equal(2, filtered.Count);
        Assert.All(filtered, p => Assert.InRange(p.Price, 2000m, 3000m));
    }

    [Fact]
    public void ApplyFilter_FiltersInStockOnly()
    {
        using var db = CreateInMemoryDbContext();

        var filter = new ProductFilter { InStockOnly = true };
        var filtered = db.Products.ApplyFilter(filter).ToList();

        Assert.Equal(4, filtered.Count);
        Assert.DoesNotContain(filtered, p => p.StockQuantity == 0);
    }

    [Fact]
    public void ApplySort_SortsByPriceDescending()
    {
        using var db = CreateInMemoryDbContext();

        var sorted = db.Products.ApplySort(sortBy: "price", sortDescending: true).ToList();

        Assert.Equal(12000m, sorted.First().Price);
        Assert.Equal(1500m, sorted.Last().Price);
    }
}
