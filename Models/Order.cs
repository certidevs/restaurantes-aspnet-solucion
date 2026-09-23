namespace RestaurantesAspNet.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double TotalPrice { get; set; }
    public double? Tip { get; set; }
    public int? TableNumber { get; set; }
    public int? NumPeople { get; set; }
    public OrderStatus Status { get; set; }

    public int RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; }

    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }

    public List<OrderLine> Lines { get; set; } = new();
}
