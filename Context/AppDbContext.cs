using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id); // Llave primaria

                entity.HasIndex(u => u.Email)
                    .IsUnique(); // Índice único para Email

                entity.HasIndex(u => u.Username)
                    .IsUnique(); // Índice único para Username

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(100); // Email es requerido y con longitud máxima

                entity.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(50); // Username es requerido y con longitud máxima

                entity.Property(u => u.Password)
                    .IsRequired();
            });

            // Configuración de la entidad Task
            modelBuilder.Entity<TaskModel>(entity =>
            {
                entity.HasKey(t => t.Id); // Llave primaria

                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(100); // Título es requerido y con longitud máxima

                entity.Property(t => t.Description)
                    .IsRequired()
                    .HasMaxLength(500); // Descripción es requerida y con longitud máxima

                entity.HasOne(t => t.User)
                    .WithMany(u => u.Tasks)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Relación de llave foránea
            });
        }
    }
}
