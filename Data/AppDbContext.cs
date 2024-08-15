using backend.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

    // Configure your model here (optional)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Customize your model here
    }
}
