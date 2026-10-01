using LabWork4.Odd.Models;
using Microsoft.EntityFrameworkCore;

namespace LabWork4.Odd.Filtering;

public static class QueryableExtensions
{
    // применение фильтрации товаров
    public static IQueryable<Product> ApplyFilter(this IQueryable<Product> query, ProductFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var search = filter.SearchTerm.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(search));
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == filter.CategoryId.Value);
        }

        if (filter.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= filter.MaxPrice.Value);
        }

        if (filter.InStockOnly.HasValue && filter.InStockOnly.Value)
        {
            query = query.Where(p => p.StockQuantity > 0);
        }

        return query;
    }

    // применение сортировки товаров
    public static IQueryable<Product> ApplySort(this IQueryable<Product> query, string? sortBy, bool sortDescending)
    {
        return (sortBy?.ToLower()) switch
        {
            "price" => sortDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
            "stock" => sortDescending ? query.OrderByDescending(p => p.StockQuantity) : query.OrderBy(p => p.StockQuantity),
            "name" => sortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            _ => sortDescending ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id)
        };
    }

    // получение страницы данных с метаинформацией
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageNumber - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        return new PagedResult<T>(items, totalCount, pageNumber, pageSize);
    }
}
