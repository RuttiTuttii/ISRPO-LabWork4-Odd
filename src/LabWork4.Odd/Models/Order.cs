namespace LabWork4.Odd.Models;

public enum OrderStatus
{
    New,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}

public class Order
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.New;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public List<OrderItem> Items { get; set; } = new();

    public decimal TotalAmount => Items.Sum(i => i.UnitPrice * i.Quantity);
}
