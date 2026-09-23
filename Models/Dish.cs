using System.ComponentModel.DataAnnotations;

namespace RestaurantesAspNet.Models;

public class Dish
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
    public string? Description { get; set; }

    [Range(0, 1000, ErrorMessage = "El precio debe estar entre 0 y 1000.")]
    public double Price { get; set; }

    public DishType DishType { get; set; }

    public int RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; }
}
