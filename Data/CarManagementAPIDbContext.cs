using CarManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CarManagementAPI.Data
{
    public class CarManagementAPIDbContext: DbContext
    {
        public CarManagementAPIDbContext(DbContextOptions<CarManagementAPIDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Car>()
        .HasMany(c => c.Garages)
        .WithMany(g => g.Cars)
        .UsingEntity<Dictionary<string, object>>(
            "CarsGarages",
            j => j.HasOne<Garage>()
                  .WithMany()
                  .HasForeignKey("GarageId")
                  .HasConstraintName("FK_CarsGarages_Garages")
                  .OnDelete(DeleteBehavior.Cascade),
            j => j.HasOne<Car>()
                  .WithMany()
                  .HasForeignKey("CarId")
                  .HasConstraintName("FK_CarsGarages_Cars")
                  .OnDelete(DeleteBehavior.Cascade)
        );


        }

        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<Garage> Garages { get; set; } = null!;
        public DbSet<Maintenance> Maintenances { get; set; } = null!;
    }
}
