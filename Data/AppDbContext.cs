using EquipmentRentalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EquipmentRentalApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Rental> Rentals { get; set; }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Equipment>().HasData(
                new Equipment { Id = 1, Name = "Excavator", Category = "Heavy Machinery", Condition = "Excellent", Description = "Large excavator for construction", RentalPrice = 500m, IsAvailable = false },
                new Equipment { Id = 2, Name = "Concrete Mixer", Category = "Construction", Condition = "Good", Description = "Portable concrete mixer", RentalPrice = 150m, IsAvailable = true },
                new Equipment { Id = 3, Name = "Power Drill", Category = "Tools", Condition = "New", Description = "Heavy duty power drill", RentalPrice = 25m, IsAvailable = true }
            );

            modelBuilder.Entity<AppUser>().HasData(
                new AppUser { Id = 1, Email = "jgohel9902@gmail.com", Role = "Admin", ExternalProvider = "Google" },
                new AppUser { Id = 2, Email = "user@example.com", Role = "User", ExternalProvider = "Google" }
            );

            modelBuilder.Entity<Rental>().HasData(
                new Rental
                {
                    Id = 1,
                    EquipmentId = 1,
                    CustomerId = 2,
                    IssuedAt = DateTime.UtcNow.AddDays(-7),
                    DueDate = DateTime.UtcNow.AddDays(-2),
                    ReturnedAt = null,
                    ReturnCondition = null,
                    Notes = "Sample active rental"
                },
                new Rental
                {
                    Id = 2,
                    EquipmentId = 3,
                    CustomerId = 2,
                    IssuedAt = DateTime.UtcNow.AddDays(-14),
                    DueDate = DateTime.UtcNow.AddDays(-8),
                    ReturnedAt = DateTime.UtcNow.AddDays(-7),
                    ReturnCondition = "Good",
                    Notes = "Completed rental"
                }
            );
        }
    }
}
