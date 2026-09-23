using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantesAspNet.Models;

namespace RestaurantesAspNet.Data;

/// <summary>Aplica migraciones y prepara roles y cuentas demo de forma idempotente.</summary>
public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // Facilita arrancar en clase sin ejecutar comandos SQL manuales.
        context.Database.Migrate();

        var configuration = services.GetRequiredService<IConfiguration>();
        if (!configuration.GetValue("SeedData:Enabled", true))
        {
            return;
        }

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await EnsureRoleAsync(roleManager, RoleNames.User);
        await EnsureRoleAsync(roleManager, RoleNames.Admin);

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        await EnsureUserAsync(
            userManager,
            username: "admin",
            email: "admin@restaurantes.local",
            displayName: "Administrador",
            password: "Admin123!",
            role: RoleNames.Admin);
        await EnsureUserAsync(
            userManager,
            username: "user",
            email: "user@restaurantes.local",
            displayName: "Usuario de demo",
            password: "User123!",
            role: RoleNames.User);

        if (!context.Restaurants.Any())
        {
            context.Restaurants.AddRange(
                new Restaurant { Name = "Casa Pepe", AveragePrice = 22.5, Active = true, NumberEmployees = 8, StartDate = new DateOnly(2015, 3, 12), FoodType = FoodType.Spanish },
                new Restaurant { Name = "Sushi Zen", AveragePrice = 35, Active = true, NumberEmployees = 12, StartDate = new DateOnly(2019, 6, 1), FoodType = FoodType.Japanese },
                new Restaurant { Name = "La Trattoria", AveragePrice = 18, Active = true, NumberEmployees = 6, StartDate = new DateOnly(2012, 10, 20), FoodType = FoodType.Italian },
                new Restaurant { Name = "El Rincón Asturiano", AveragePrice = 28, Active = true, NumberEmployees = 10, StartDate = new DateOnly(2008, 5, 15), FoodType = FoodType.Spanish },
                new Restaurant { Name = "Burger Station", AveragePrice = 12.9, Active = false, NumberEmployees = 4, StartDate = new DateOnly(2021, 1, 8), FoodType = FoodType.American });
            context.SaveChanges();
        }

        if (!context.Employees.Any())
        {
            var casaPepe = context.Restaurants.First(r => r.Name == "Casa Pepe");
            var sushiZen = context.Restaurants.First(r => r.Name == "Sushi Zen");
            var trattoria = context.Restaurants.First(r => r.Name == "La Trattoria");

            context.Employees.AddRange(
                new Employee { FirstName = "Lucía", LastName = "Martínez", Dni = "12345678A", Age = 29, Restaurant = casaPepe },
                new Employee { FirstName = "Javier", LastName = "Gómez", Dni = "23456789B", Age = 41, Restaurant = casaPepe },
                new Employee { FirstName = "Yuki", LastName = "Tanaka", Dni = "34567890C", Age = 35, Restaurant = sushiZen },
                new Employee { FirstName = "Marco", LastName = "Rossi", Dni = "45678901D", Age = 38, Restaurant = trattoria },
                new Employee { FirstName = "Ana", LastName = "López", Age = 24, Restaurant = trattoria });
            context.SaveChanges();
        }

        if (!context.Dishes.Any())
        {
            var casaPepe = context.Restaurants.First(r => r.Name == "Casa Pepe");
            var sushiZen = context.Restaurants.First(r => r.Name == "Sushi Zen");
            var trattoria = context.Restaurants.First(r => r.Name == "La Trattoria");
            var rincon = context.Restaurants.First(r => r.Name == "El Rincón Asturiano");

            context.Dishes.AddRange(
                new Dish { Name = "Croquetas de jamón", Description = "Ocho croquetas caseras", Price = 9.5, DishType = DishType.Starter, Restaurant = casaPepe },
                new Dish { Name = "Paella de marisco", Description = "Para dos personas", Price = 32, DishType = DishType.MainCourse, Restaurant = casaPepe },
                new Dish { Name = "Tarta de queso", Price = 6, DishType = DishType.Dessert, Restaurant = casaPepe },
                new Dish { Name = "Gyozas", Description = "Seis empanadillas a la plancha", Price = 8, DishType = DishType.Starter, Restaurant = sushiZen },
                new Dish { Name = "Menú de sushi", Description = "Doce piezas variadas", Price = 24, DishType = DishType.MainCourse, Restaurant = sushiZen },
                new Dish { Name = "Mochi de té verde", Price = 5.5, DishType = DishType.Dessert, Restaurant = sushiZen },
                new Dish { Name = "Burrata", Description = "Con tomate y albahaca", Price = 11, DishType = DishType.Starter, Restaurant = trattoria },
                new Dish { Name = "Pizza margarita", Price = 12, DishType = DishType.MainCourse, Restaurant = trattoria },
                new Dish { Name = "Tiramisú", Price = 6.5, DishType = DishType.Dessert, Restaurant = trattoria },
                new Dish { Name = "Fabada asturiana", Description = "Con compango", Price = 16, DishType = DishType.MainCourse, Restaurant = rincon },
                new Dish { Name = "Cachopo", Description = "De ternera con jamón y queso", Price = 22, DishType = DishType.MainCourse, Restaurant = rincon },
                new Dish { Name = "Arroz con leche", Price = 5, DishType = DishType.Dessert, Restaurant = rincon });
            context.SaveChanges();
        }

        if (!context.Reviews.Any())
        {
            var casaPepe = context.Restaurants.First(r => r.Name == "Casa Pepe");
            var sushiZen = context.Restaurants.First(r => r.Name == "Sushi Zen");
            var rincon = context.Restaurants.First(r => r.Name == "El Rincón Asturiano");

            context.Reviews.AddRange(
                new Review { Comment = "La paella, espectacular. Volveremos.", Rating = 5, Date = new DateTime(2026, 9, 1), Restaurant = casaPepe },
                new Review { Comment = "Buena comida, pero tardaron en atendernos.", Rating = 3, Date = new DateTime(2026, 9, 5), Restaurant = casaPepe },
                new Review { Comment = "El mejor sushi de la ciudad.", Rating = 5, Date = new DateTime(2026, 8, 20), Restaurant = sushiZen },
                new Review { Comment = "Cachopo enorme y muy rico.", Rating = 4, Date = new DateTime(2026, 9, 10), Restaurant = rincon });
            context.SaveChanges();
        }

        Console.WriteLine($"Restaurantes en la base de datos: {context.Restaurants.Count()}");
        Console.WriteLine($"Empleados en la base de datos: {context.Employees.Count()}");
        Console.WriteLine($"Platos en la base de datos: {context.Dishes.Count()}");
        Console.WriteLine($"Reseñas en la base de datos: {context.Reviews.Count()}");
    }

    private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var result = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"No se pudo crear el rol {roleName}.");
            }
        }
    }

    private static async Task EnsureUserAsync(
        UserManager<ApplicationUser> userManager,
        string username,
        string email,
        string displayName,
        string password,
        string role)
    {
        var user = await userManager.FindByNameAsync(username);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                DisplayName = displayName,
                EmailConfirmed = true,
                IsActive = true
            };
            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException($"No se pudo crear el usuario demo {username}.");
            }
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            var roleResult = await userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException($"No se pudo asignar el rol {role} a {username}.");
            }
        }
    }
}
