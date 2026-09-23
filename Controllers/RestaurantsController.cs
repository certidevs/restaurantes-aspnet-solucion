using Microsoft.AspNetCore.Mvc;
using RestaurantesAspNet.Data;

namespace RestaurantesAspNet.Controllers;

public class RestaurantsController : Controller
{
    private readonly ApplicationDbContext context;

    public RestaurantsController(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IActionResult Index()
    {
        var restaurants = context.Restaurants.ToList();
        return View(restaurants);
    }
}
