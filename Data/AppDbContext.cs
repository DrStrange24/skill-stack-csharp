using WebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

    // Configure your model here (optional)
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);  // Ensure the Identity configurations are applied

        // Further customization if needed
    }
}
