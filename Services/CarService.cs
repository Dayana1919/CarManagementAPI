using CarManagementAPI.Contracts;
using CarManagementAPI.Data;
using CarManagementAPI.DTOs.Car;
using CarManagementAPI.DTOs.Garage;
using CarManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CarManagementAPI.Services
{
    public class CarService : ICarService
    {
        private readonly CarManagementAPIDbContext _context;

        public CarService(CarManagementAPIDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ResponseCarDto>> GetAllAsync()
        {
            var cars = await _context.Cars.Include(c => c.Garages).ToListAsync();

            return cars.Select(car => new ResponseCarDto
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                ProductionYear = car.ProductionYear,
                LicensePlate = car.LicensePlate,
                Garages = car.Garages?.Select(g => new ResponseGarageDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Location = g.Location,
                    City = g.City,
                    Capacity = g.Capacity
                }).ToList()
            });
        }

        public async Task<ResponseCarDto> GetByIdAsync(int id)
        {
            var car = await _context.Cars.Include(c => c.Garages).FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
            {
                throw new KeyNotFoundException("Car not found");
            }

            return new ResponseCarDto
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                ProductionYear = car.ProductionYear,
                LicensePlate = car.LicensePlate,
                Garages = car.Garages.Select(g => new ResponseGarageDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Location = g.Location,
                    City = g.City,
                    Capacity = g.Capacity
                }).ToList()
            };
        }

        public async Task<int> CreateAsync(CreateCarDto carDto)
        {
            var car = new Car
            {
                Make = carDto.Make,
                Model = carDto.Model,
                ProductionYear = carDto.ProductionYear,
                LicensePlate = carDto.LicensePlate
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            if (carDto.GarageIds.Any())
            {
                var garages = await _context.Garages
                    .Where(g => carDto.GarageIds.Contains(g.Id))
                    .ToListAsync();

                car.Garages = garages;
                await _context.SaveChangesAsync();
            }

            return car.Id;
        }

        public async Task UpdateAsync(int id, UpdateCarDto carDto)
        {
            var car = await _context.Cars.Include(c => c.Garages).FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
            {
                throw new KeyNotFoundException("Car not found");
            }

            car.Make = carDto.Make;
            car.Model = carDto.Model;
            car.ProductionYear = carDto.ProductionYear;
            car.LicensePlate = carDto.LicensePlate;

            if (carDto.GarageIds.Any())
            {
                var garages = await _context.Garages
                    .Where(g => carDto.GarageIds.Contains(g.Id))
                    .ToListAsync();

                car.Garages = garages;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Car car)
        {
            var existingCar = await _context.Cars.FindAsync(car.Id);

            if (existingCar == null)
            {
                throw new KeyNotFoundException("Car not found");
            }

            _context.Cars.Remove(existingCar);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ResponseCarDto>> GetFilteredAsync(string? carMake, int? fromYear, int? toYear, int? garageId)
        {
            var query = _context.Cars.Include(c => c.Garages).AsQueryable();


            if (!string.IsNullOrEmpty(carMake))
            {
                query = query.Where(c => c.Make.ToLower().Contains(carMake.ToLower()));
            }

            if (fromYear.HasValue)
            {
                query = query.Where(c => c.ProductionYear >= fromYear.Value);
            }

            if (toYear.HasValue)
            {
                query = query.Where(c => c.ProductionYear <= toYear.Value);
            }

            if (garageId.HasValue)
            {
                query = query.Where(c => c.Garages.Any(g => g.Id == garageId.Value));
            }

            var cars = await query.ToListAsync();

            return cars.Select(car => new ResponseCarDto
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                ProductionYear = car.ProductionYear,
                LicensePlate = car.LicensePlate,
                Garages = car.Garages.Select(g => new ResponseGarageDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Location = g.Location,
                    City = g.City,
                    Capacity = g.Capacity
                }).ToList()
            });
        }
    }
}
