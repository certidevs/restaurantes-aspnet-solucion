namespace RestaurantesAspNet.Models;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Dni { get; set; }
    public int? Age { get; set; }

    public int RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; }
}
