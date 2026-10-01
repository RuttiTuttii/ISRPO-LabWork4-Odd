namespace LabWork4.Odd.Models;

public class Customer
{
    private string _name = string.Empty;
    private string _email = string.Empty;

    public int Id { get; set; }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("имя клиента не может быть пустым", nameof(value));
            _name = value.Trim();
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
                throw new ArgumentException("некорректный адрес электронной почты", nameof(value));
            _email = value.Trim().ToLowerInvariant();
        }
    }

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    public List<Order> Orders { get; set; } = new();
}
