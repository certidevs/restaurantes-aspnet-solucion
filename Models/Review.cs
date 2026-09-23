using System.ComponentModel.DataAnnotations;

namespace RestaurantesAspNet.Models;

public class Review
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Escribe un comentario.")]
    [StringLength(1000, ErrorMessage = "El comentario no puede superar los 1000 caracteres.")]
    public string Comment { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5.")]
    public int Rating { get; set; }

    public DateTime Date { get; set; }

    public int RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; }

    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
}
