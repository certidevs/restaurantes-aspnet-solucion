using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantesAspNet.Data;
using RestaurantesAspNet.Models;

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

    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet]
    public IActionResult Create(int? restaurantId)
    {
        var dish = new Dish();
        if (restaurantId != null)
        {
            dish.RestaurantId = restaurantId.Value;
        }

        LoadRestaurants();
        return View("Form", dish);
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var dish = context.Dishes.Find(id);
        if (dish == null)
        {
            return NotFound();
        }

        LoadRestaurants();
        return View("Form", dish);
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPost]
    public IActionResult Save(Dish dish)
    {
        if (!ModelState.IsValid)
        {
            LoadRestaurants();
            return View("Form", dish);
        }

        if (dish.Id == 0)
        {
            context.Dishes.Add(dish);
        }
        else
        {
            context.Dishes.Update(dish);
        }

        context.SaveChanges();

        TempData["Message"] = "Plato guardado correctamente.";
        return RedirectToAction("Details", new { id = dish.Id });
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var dish = context.Dishes.Find(id);
        if (dish == null)
        {
            return NotFound();
        }

        context.Dishes.Remove(dish);
        context.SaveChanges();

        TempData["Message"] = "Plato borrado.";
        return RedirectToAction("Details", "Restaurants", new { id = dish.RestaurantId });
    }

    private void LoadRestaurants()
    {
        var restaurants = context.Restaurants.OrderBy(r => r.Name).ToList();
        ViewBag.Restaurants = new SelectList(restaurants, "Id", "Name");
    }
}
