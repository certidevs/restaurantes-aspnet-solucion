using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantesAspNet.Data;
using RestaurantesAspNet.Models;

namespace RestaurantesAspNet.Controllers;

public class ReviewsController : Controller
{
    private readonly ApplicationDbContext context;

    public ReviewsController(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IActionResult Index()
    {
        var reviews = context.Reviews
            .Include(r => r.Restaurant)
            .OrderByDescending(r => r.Date)
            .ToList();
        return View(reviews);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Create(int restaurantId)
    {
        var restaurant = context.Restaurants.Find(restaurantId);
        if (restaurant == null)
        {
            return NotFound();
        }

        ViewBag.RestaurantName = restaurant.Name;
        var review = new Review { RestaurantId = restaurantId, Rating = 5 };
        return View(review);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Save(Review review)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.RestaurantName = context.Restaurants.Find(review.RestaurantId)?.Name;
            return View("Create", review);
        }

        review.Date = DateTime.Now;
        context.Reviews.Add(review);
        context.SaveChanges();

        TempData["Message"] = "Gracias por tu reseña.";
        return RedirectToAction("Details", "Restaurants", new { id = review.RestaurantId });
    }
}
