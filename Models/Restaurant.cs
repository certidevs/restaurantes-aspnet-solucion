using System.ComponentModel.DataAnnotations;

namespace RestaurantesAspNet.Models;

public class Restaurant
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Range(0, 1000, ErrorMessage = "El precio medio debe estar entre 0 y 1000.")]
    public double? AveragePrice { get; set; }

    public bool Active { get; set; }

    [Range(0, 500, ErrorMessage = "El número de empleados debe estar entre 0 y 500.")]
    public int? NumberEmployees { get; set; }

    public DateOnly? StartDate { get; set; }
    public FoodType? FoodType { get; set; }

    public List<Employee> Employees { get; set; } = new();
    public List<Dish> Dishes { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
}
