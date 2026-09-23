using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantesAspNet.Data;

namespace RestaurantesAspNet.Controllers;

public class DishesController : Controller
{
    private readonly ApplicationDbContext context;

    public DishesController(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IActionResult Index()
    {
        var dishes = context.Dishes
            .Include(d => d.Restaurant)
            .OrderBy(d => d.Name)
            .ToList();
        return View(dishes);
    }

    public IActionResult Details(int id)
    {
        var dish = context.Dishes
            .Include(d => d.Restaurant)
            .FirstOrDefault(d => d.Id == id);
        if (dish == null)
        {
            return NotFound();
        }

        return View(dish);
    }
}
