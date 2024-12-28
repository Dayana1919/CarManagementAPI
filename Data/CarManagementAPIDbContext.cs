using CarManagementAPI.Models;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using CarManagementAPI.Data.Models;

namespace CarManagementAPI.Data
{
    public class CarManagementAPIDbContext: DbContext
    {
        public CarManagementAPIDbContext(DbContextOptions<CarManagementAPIDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Конфигурация на много към много връзката между Car и Garage
            builder.Entity<CarGarage>()
                .HasKey(e => new { e.CarId, e.GarageId });

            builder.Entity<CarGarage>()
                .HasOne(e => e.Car)
                .WithMany(c => c.CarGarages)
                .HasForeignKey(e => e.CarId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CarGarage>()
                .HasOne(e => e.Garage)
                .WithMany(g => g.CarsGarage)
                .HasForeignKey(e => e.GarageId)
                .OnDelete(DeleteBehavior.NoAction);

            // Конфигурация за Maintenance
            builder.Entity<Maintenance>()
                .HasOne(m => m.Car)
                .WithMany()
                .HasForeignKey(m => m.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Maintenance>()
                .HasOne(m => m.Garage)
                .WithMany()
                .HasForeignKey(m => m.GarageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Maintenance>()
                .HasIndex(m => new { m.CarId, m.ScheduledDate })
                .IsUnique();

            base.OnModelCreating(builder);
        }

        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<Garage> Garages { get; set; } = null!;
        public DbSet<Maintenance> Maintenances { get; set; } = null!;
        public DbSet<CarGarage> CarsGarages { get; set; } = null!;
    }
}
