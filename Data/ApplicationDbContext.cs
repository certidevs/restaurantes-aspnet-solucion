using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantesAspNet.Models;

namespace RestaurantesAspNet.Data;

/// <summary>
/// Contexto de EF Core: configura SQLite y reúne las tablas de la aplicación.
/// Las entidades de restaurante se añadirán aquí durante las clases.
/// </summary>
public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    /// <summary>EF Core recibe estas opciones desde Program.cs para conectar con SQLite.</summary>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<Employee> Employees => Set<Employee>();

    /// <summary>Configura las columnas añadidas a la tabla de usuarios de Identity.</summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(user => user.DisplayName).HasMaxLength(100);
            entity.Property(user => user.AvatarFileName).HasMaxLength(260);
            entity.Property(user => user.CreatedAt).IsRequired();
        });
    }
}
