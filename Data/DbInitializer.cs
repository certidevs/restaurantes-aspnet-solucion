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

        Console.WriteLine($"Restaurantes en la base de datos: {context.Restaurants.Count()}");
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
