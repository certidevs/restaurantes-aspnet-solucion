using System.ComponentModel.DataAnnotations;

namespace RestaurantesAspNet.Models;

public enum OrderStatus
{
    [Display(Name = "Pendiente")]
    Pending,

    [Display(Name = "En curso")]
    InProgress,

    [Display(Name = "Completado")]
    Completed
}
