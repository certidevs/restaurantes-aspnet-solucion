using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantesAspNet.Data;
using RestaurantesAspNet.Models;

namespace RestaurantesAspNet.Controllers;

public class RestaurantsController : Controller
{
    private readonly ApplicationDbContext context;

    public RestaurantsController(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IActionResult Index(string? search, FoodType? foodType, double? maxPrice)
    {
        var query = context.Restaurants.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(r => r.Name.ToLower().Contains(search.ToLower()));
        }

        if (foodType != null)
        {
            query = query.Where(r => r.FoodType == foodType);
        }

        if (maxPrice != null)
        {
            query = query.Where(r => r.AveragePrice <= maxPrice);
        }

        ViewBag.Search = search;
        ViewBag.FoodType = foodType;
        ViewBag.MaxPrice = maxPrice;

        var restaurants = query.OrderBy(r => r.Name).ToList();
        return View(restaurants);
    }

    public IActionResult Details(int id)
    {
        var restaurant = context.Restaurants
            .Include(r => r.Employees)
            .Include(r => r.Dishes)
            .Include(r => r.Reviews)
            .FirstOrDefault(r => r.Id == id);
        if (restaurant == null)
        {
            return NotFound();
        }

        return View(restaurant);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var restaurant = new Restaurant { Active = true };
        return View("Form", restaurant);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var restaurant = context.Restaurants.Find(id);
        if (restaurant == null)
        {
            return NotFound();
        }

        return View("Form", restaurant);
    }

    [HttpPost]
    public IActionResult Save(Restaurant restaurant)
    {
        if (!ModelState.IsValid)
        {
            return View("Form", restaurant);
        }

        if (restaurant.Id == 0)
        {
            context.Restaurants.Add(restaurant);
        }
        else
        {
            context.Restaurants.Update(restaurant);
        }

        context.SaveChanges();

        TempData["Message"] = "Restaurante guardado correctamente.";
        return RedirectToAction("Details", new { id = restaurant.Id });
    }

    [HttpPost]
    public IActionResult Deactivate(int id)
    {
        var restaurant = context.Restaurants.Find(id);
        if (restaurant == null)
        {
            return NotFound();
        }

        restaurant.Active = false;
        context.SaveChanges();

        TempData["Message"] = "Restaurante desactivado.";
        return RedirectToAction("Details", new { id });
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var restaurant = context.Restaurants.Find(id);
        if (restaurant == null)
        {
            return NotFound();
        }

        context.Restaurants.Remove(restaurant);
        context.SaveChanges();

        TempData["Message"] = "Restaurante borrado.";
        return RedirectToAction("Index");
    }
}
