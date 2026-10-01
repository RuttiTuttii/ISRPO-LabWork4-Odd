using System.Globalization;
using LabWork4.Odd.Data;
using LabWork4.Odd.Filtering;
using Microsoft.EntityFrameworkCore;

// настраиваем кодировку консоли и культуру для SQLite
Console.OutputEncoding = System.Text.Encoding.UTF8;
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

Console.WriteLine("==========================================================");
Console.WriteLine("  ИСРПО ЛР №4 — Команда Нечетных (Модели, БД, Пагинация)");
Console.WriteLine("==========================================================");

// инициализируем контекст базы данных и сидируем начальные данные
using var db = new AppDbContext();
DbInitializer.Initialize(db);

Console.WriteLine($"\n[БД инициализирована]");
Console.WriteLine($"Категорий в каталоге: {await db.Categories.CountAsync()}");
Console.WriteLine($"Товаров на складе: {await db.Products.CountAsync()}");
Console.WriteLine($"Зарегистрировано клиентов: {await db.Customers.CountAsync()}");
Console.WriteLine($"Заказов в системе: {await db.Orders.CountAsync()}");

var task = args.FirstOrDefault();
if (string.IsNullOrEmpty(task))
{
    Console.WriteLine("\nВыберите сценарий тестирования:");
    Console.WriteLine("1 - Вывод всех категорий и связанных товаров (Задача 1, Участник 1)");
    Console.WriteLine("2 - Демонстрация пагинации (страница 1 и 2 по 3 товара) (Задача 3, Участник 3)");
    Console.WriteLine("3 - Фильтрация по категории 'Ноутбуки' и сортировка по убыванию цены");
    Console.WriteLine("4 - Поиск по слову 'pro' и проверка наличия на складе");
    Console.WriteLine("all - Запуск всех сценариев");
    Console.Write("\nНомер сценария [all]: ");

    var input = Console.ReadLine()?.Trim();
    task = string.IsNullOrEmpty(input) ? "all" : input;
}

switch (task.ToLower())
{
    case "1":
        await ShowCategoriesAndProducts(db);
        break;

    case "2":
        await ShowPaginationDemo(db);
        break;

    case "3":
        await ShowFilterAndSortDemo(db);
        break;

    case "4":
        await ShowSearchDemo(db);
        break;

    case "all":
    default:
        await ShowCategoriesAndProducts(db);
        await ShowPaginationDemo(db);
        await ShowFilterAndSortDemo(db);
        await ShowSearchDemo(db);
        break;
}

static async Task ShowCategoriesAndProducts(AppDbContext db)
{
    Console.WriteLine("\n--- [Сценарий 1] Категории и товары (Задача 1) ---");
    var categories = await db.Categories.Include(c => c.Products).ToListAsync();
    foreach (var cat in categories)
    {
        Console.WriteLine($"\n📁 Категория: {cat.Name} ({cat.Description})");
        foreach (var prod in cat.Products)
        {
            Console.WriteLine($"   • {prod.Name} | Цена: {prod.Price:N2} ₽ | Остаток: {prod.StockQuantity} шт.");
        }
    }
}

static async Task ShowPaginationDemo(AppDbContext db)
{
    Console.WriteLine("\n--- [Сценарий 2] Пагинация каталога (Задача 3) ---");
    var pageSize = 3;
    for (int page = 1; page <= 3; page++)
    {
        var result = await db.Products
            .AsNoTracking()
            .ApplySort("price", sortDescending: false)
            .ToPagedResultAsync(page, pageSize);

        Console.WriteLine($"\nСтраница {result.PageNumber} из {result.TotalPages} (Всего товаров: {result.TotalCount}):");
        foreach (var item in result.Items)
        {
            Console.WriteLine($"   [{item.Id}] {item.Name} — {item.Price:N2} ₽");
        }
        Console.WriteLine($"   Назад: {(result.HasPreviousPage ? "Да" : "Нет")}, Вперед: {(result.HasNextPage ? "Да" : "Нет")}");
    }
}

static async Task ShowFilterAndSortDemo(AppDbContext db)
{
    Console.WriteLine("\n--- [Сценарий 3] Фильтрация и сортировка (Задача 3) ---");
    var filter = new ProductFilter
    {
        CategoryId = 1, // Ноутбуки
        MinPrice = 90000m,
        SortBy = "price",
        SortDescending = true
    };

    var result = await db.Products
        .AsNoTracking()
        .ApplyFilter(filter)
        .ApplySort(filter.SortBy, filter.SortDescending)
        .ToPagedResultAsync(1, 10);

    Console.WriteLine($"Найдено ноутбуков дороже 90 000 ₽: {result.TotalCount} (сортировка по убыванию цены):");
    foreach (var item in result.Items)
    {
        Console.WriteLine($"   • {item.Name} — {item.Price:N2} ₽ (Остаток: {item.StockQuantity})");
    }
}

static async Task ShowSearchDemo(AppDbContext db)
{
    Console.WriteLine("\n--- [Сценарий 4] Поиск по ключевому слову 'pro' (Задача 3) ---");
    var filter = new ProductFilter
    {
        SearchTerm = "pro",
        InStockOnly = true
    };

    var result = await db.Products
        .AsNoTracking()
        .ApplyFilter(filter)
        .ToPagedResultAsync(1, 10);

    Console.WriteLine($"Результаты поиска по '{filter.SearchTerm}': {result.TotalCount} позиций:");
    foreach (var item in result.Items)
    {
        Console.WriteLine($"   • {item.Name} — {item.Price:N2} ₽");
    }
}
