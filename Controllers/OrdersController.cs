using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantesAspNet.Data;
using RestaurantesAspNet.Models;
using RestaurantesAspNet.Utilities;

namespace RestaurantesAspNet.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly ApplicationDbContext context;

    public OrdersController(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IActionResult Index()
    {
        var query = context.Orders
            .Include(o => o.Restaurant)
            .Include(o => o.User)
            .AsQueryable();

        if (!User.IsInRole(RoleNames.Admin))
        {
            var userId = User.GetRequiredUserId();
            query = query.Where(o => o.UserId == userId);
        }

        var orders = query.OrderByDescending(o => o.Date).ToList();
        return View(orders);
    }

    public IActionResult Details(int id)
    {
        var order = context.Orders
            .Include(o => o.Restaurant)
            .Include(o => o.User)
            .Include(o => o.Lines).ThenInclude(l => l.Dish)
            .FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            return NotFound();
        }

        if (order.UserId != User.GetRequiredUserId() && !User.IsInRole(RoleNames.Admin))
        {
            return Forbid();
        }

        return View(order);
    }
}
