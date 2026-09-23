using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public IActionResult Details(int id)
    {
        var restaurant = context.Restaurants
            .Include(r => r.Employees)
            .FirstOrDefault(r => r.Id == id);
        if (restaurant == null)
        {
            return NotFound();
        }

        return View(restaurant);
    }
}
