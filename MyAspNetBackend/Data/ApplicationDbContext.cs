using Microsoft.EntityFrameworkCore;
using MyAspNetBackend.Models;

namespace MyAspNetBackend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2); // 18 digits total, with 2 decimal places
    }
}