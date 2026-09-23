namespace RestaurantesAspNet.Models;

public class OrderLine
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    public int DishId { get; set; }
    public Dish? Dish { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }
}
