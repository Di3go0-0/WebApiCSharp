using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<WebApi.Models.Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id); // Primary key

                entity.HasIndex(u => u.Email)
                    .IsUnique(); // Unique index on Email
                
                entity.HasIndex(u => u.Username)
                    .IsUnique(); // Unique index on Username
                
                entity.Property(u => u.Email)
                    .IsRequired(); // Email is required
                
                entity.Property(u => u.Username)
                    .IsRequired(); // Username is required

                entity.Property(u => u.Password)
                    .IsRequired(); // Password is required
            });

            // Configure Task entity
            modelBuilder.Entity<WebApi.Models.Task>(entity =>
            {
                entity.HasKey(t => t.Id); // Primary key

                entity.Property(t => t.Title)
                    .IsRequired(); // Title is required
                
                entity.Property(t => t.Description)
                    .IsRequired(); // Description is required
                
                entity.HasOne(t => t.User)
                    .WithMany(u => u.Tasks)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Foreign key relationship
            });
        }
    }
}