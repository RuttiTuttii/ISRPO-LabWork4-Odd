namespace LabWork4.Odd.Models;

public class Product
{
    private string _name = string.Empty;
    private decimal _price;

    public int Id { get; set; }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("название товара не может быть пустым", nameof(value));
            _name = value.Trim();
        }
    }

    public decimal Price
    {
        get => _price;
        set
        {
            if (value < 0)
                throw new ArgumentException("цена товара не может быть отрицательной", nameof(value));
            _price = value;
        }
    }

    public int StockQuantity { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
