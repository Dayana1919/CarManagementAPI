using CarManagementAPI.Models;
using System.Collections.Generic;
using System;
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
            // Define relationships and keys
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

            base.OnModelCreating(builder);
        }

        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<Garage> Garages { get; set; } = null!;
        public DbSet<Maintenance> Maintenances { get; set; } = null!;
    }
}
