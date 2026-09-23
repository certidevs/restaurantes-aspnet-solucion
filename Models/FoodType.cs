using System.ComponentModel.DataAnnotations;

namespace RestaurantesAspNet.Models;

public enum FoodType
{
    [Display(Name = "Española")]
    Spanish,

    [Display(Name = "Japonesa")]
    Japanese,

    [Display(Name = "Italiana")]
    Italian,

    [Display(Name = "Americana")]
    American,

    [Display(Name = "Mexicana")]
    Mexican
}
