namespace RestaurantesAspNet.Models;

public class Restaurant
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double? AveragePrice { get; set; }
    public bool Active { get; set; }
    public int? NumberEmployees { get; set; }
}
