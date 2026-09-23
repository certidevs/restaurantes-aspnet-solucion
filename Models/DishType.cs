using System.ComponentModel.DataAnnotations;

namespace RestaurantesAspNet.Models;

public enum DishType
{
    [Display(Name = "Entrante")]
    Starter,

    [Display(Name = "Principal")]
    MainCourse,

    [Display(Name = "Postre")]
    Dessert
}
