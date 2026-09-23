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

        if (order.Status != OrderStatus.Completed)
        {
            ViewBag.Dishes = context.Dishes
                .Where(d => d.RestaurantId == order.RestaurantId)
                .OrderBy(d => d.DishType)
                .ThenBy(d => d.Name)
                .ToList();
        }

        return View(order);
    }

    [HttpPost]
    public IActionResult Create(int restaurantId)
    {
        var order = new Order
        {
            Date = DateTime.Now,
            Status = OrderStatus.Pending,
            RestaurantId = restaurantId,
            UserId = User.GetRequiredUserId()
        };
        context.Orders.Add(order);
        context.SaveChanges();

        return RedirectToAction("Details", new { id = order.Id });
    }

    [HttpPost]
    public IActionResult AddDish(int orderId, int dishId)
    {
        var order = FindOpenOrder(orderId);
        if (order == null)
        {
            return NotFound();
        }

        var line = order.Lines.FirstOrDefault(l => l.DishId == dishId);
        if (line == null)
        {
            order.Lines.Add(new OrderLine { DishId = dishId, Quantity = 1 });
        }
        else
        {
            line.Quantity++;
        }

        order.Status = OrderStatus.InProgress;
        context.SaveChanges();

        UpdateTotal(order.Id);
        return RedirectToAction("Details", new { id = orderId });
    }

    [HttpPost]
    public IActionResult RemoveLine(int orderId, int lineId)
    {
        var order = FindOpenOrder(orderId);
        if (order == null)
        {
            return NotFound();
        }

        var line = order.Lines.FirstOrDefault(l => l.Id == lineId);
        if (line != null)
        {
            context.OrderLines.Remove(line);
            context.SaveChanges();
        }

        UpdateTotal(order.Id);
        return RedirectToAction("Details", new { id = orderId });
    }

    private Order? FindOpenOrder(int orderId)
    {
        var userId = User.GetRequiredUserId();
        return context.Orders
            .Include(o => o.Lines)
            .FirstOrDefault(o => o.Id == orderId && o.UserId == userId && o.Status != OrderStatus.Completed);
    }

    private void UpdateTotal(int orderId)
    {
        var order = context.Orders
            .Include(o => o.Lines).ThenInclude(l => l.Dish)
            .First(o => o.Id == orderId);
        order.TotalPrice = order.Lines.Sum(l => l.Dish!.Price * l.Quantity) + (order.Tip ?? 0);
        context.SaveChanges();
    }
}
