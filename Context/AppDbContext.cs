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

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email)
                    .IsUnique();
            });

            modelBuilder.Entity<WebApi.Models.Task>(entity =>
            {
                entity.HasKey(t => t.Id); // Clave primaria

                entity.Property(t => t.Title)
                    .IsRequired();

                entity.HasOne(t => t.User)
                    .WithMany(u => u.Tasks)
                    .HasForeignKey(t => t.UserId) // Clave foránea
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}