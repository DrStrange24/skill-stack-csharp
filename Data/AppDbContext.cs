using PersonalWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Message> Messages { get; set; }

    // Configure your model here (optional)
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);  // Ensure the Identity configurations are applied

        MessageConfig(builder);
    }

    private void MessageConfig(ModelBuilder builder)
    {
        // Configure MessageId to have a default GUID value
        //builder.Entity<Message>()
        //    .Property(m => m.MessageId)
        //    .HasDefaultValueSql("UUID()");

        //// Alternatively, you can enforce GUID generation in the application code by setting the default value in the class constructor
        //builder.Entity<Message>()
        //    .Property(m => m.SentAt)
        //    .HasDefaultValueSql("CURRENT_TIMESTAMP");  // Default for SentAt

        //builder.Entity<Message>()
        //    .HasOne(m => m.Sender)
        //    .WithMany() // No navigation property in IdentityUser for sent messages
        //    .HasForeignKey(m => m.SenderId)
        //    .OnDelete(DeleteBehavior.Cascade);

        //builder.Entity<Message>()
        //    .HasOne(m => m.Receiver)
        //    .WithMany() // No navigation property in IdentityUser for received messages
        //    .HasForeignKey(m => m.ReceiverId)
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}
