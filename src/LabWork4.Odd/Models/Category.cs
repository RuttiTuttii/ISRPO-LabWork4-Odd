namespace LabWork4.Odd.Models;

public class Category
{
    private string _name = string.Empty;

    public int Id { get; set; }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("название категории не может быть пустым", nameof(value));
            _name = value.Trim();
        }
    }

    public string Description { get; set; } = string.Empty;

    public List<Product> Products { get; set; } = new();
}
