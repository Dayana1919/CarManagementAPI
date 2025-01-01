using CarManagementAPI.Contracts;
using CarManagementAPI.Data;
using CarManagementAPI.DTOs.Garage;
using CarManagementAPI.DTOs.Reports;
using CarManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;


namespace CarManagementAPI.Services
{
    public class GarageService: IGarageService
    {
        private readonly CarManagementAPIDbContext _context;

        public GarageService(CarManagementAPIDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ResponseGarageDto>> GetAllAsync()
        {
            var garages = await _context.Garages.ToListAsync();

            return garages.Select(garage => new ResponseGarageDto
            {
                Id = garage.Id,
                Name = garage.Name,
                Location = garage.Location,
                City = garage.City,
                Capacity = garage.Capacity
            });
        }

        public async Task<ResponseGarageDto> GetByIdAsync(int id)
        {
            var garage = await _context.Garages.FirstOrDefaultAsync(g => g.Id == id);

            if (garage == null)
            {
                throw new KeyNotFoundException("Garage not found");
            }

            return new ResponseGarageDto
            {
                Id = garage.Id,
                Name = garage.Name,
                Location = garage.Location,
                City = garage.City,
                Capacity = garage.Capacity
            };
        }

        public async Task<int> CreateAsync(Garage garage)
        {
            _context.Garages.Add(garage);
            await _context.SaveChangesAsync();

            return garage.Id;
        }

        public async Task UpdateAsync(int id, UpdateGarageDto garageDto)
        {
            var garage = await _context.Garages.FirstOrDefaultAsync(g => g.Id == id);

            if (garage == null)
            {
                throw new KeyNotFoundException("Garage not found");
            }

            garage.Name = garageDto.Name;
            garage.Location = garageDto.Location;
            garage.City = garageDto.City;
            garage.Capacity = garageDto.Capacity;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Garage garage)
        {
            var existingGarage = await _context.Garages.FindAsync(garage.Id);

            if (existingGarage == null)
            {
                throw new KeyNotFoundException("Garage not found");
            }

            _context.Garages.Remove(existingGarage);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ResponseGarageDto>> GetFilteredAsync(string? city)
        {
            var query = _context.Garages.AsQueryable();

            if (!string.IsNullOrEmpty(city))
            {
                query = query.Where(g => g.City.Contains(city));
            }

            var garages = await query.ToListAsync();

            return garages.Select(garage => new ResponseGarageDto
            {
                Id = garage.Id,
                Name = garage.Name,
                Location = garage.Location,
                City = garage.City,
                Capacity = garage.Capacity
            });
        }

        public async Task<IEnumerable<GarageDailyAvailabilityReportDto>> GenerateDailyAvailabilityReportAsync(int garageId, string startDate, string endDate)
        {
            if (!DateTime.TryParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var start) ||
                !DateTime.TryParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var end))
            {
                throw new ArgumentException("Invalid date format for startDate or endDate. Use 'yyyy-MM-dd'.");
            }

            // Fetch garage information
            var garage = await _context.Garages.FindAsync(garageId);
            if (garage == null)
            {
                throw new KeyNotFoundException($"Garage with ID {garageId} not found.");
            }

            // Fetch maintenance records for the given garage and date range
            var maintenances = await _context.Maintenances
                .Where(m => m.GarageId == garageId && m.ScheduledDate >= start && m.ScheduledDate <= end)
                .ToListAsync();

            // Count requests per day
            var dailyRequestCounts = maintenances
                .GroupBy(m => m.ScheduledDate.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            // Generate report
            var report = new List<GarageDailyAvailabilityReportDto>();
            for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
            {
                var requests = dailyRequestCounts.ContainsKey(date) ? dailyRequestCounts[date] : 0;
                var availableCapacity = garage.Capacity - requests;

                report.Add(new GarageDailyAvailabilityReportDto
                {
                    Date = date.ToString("yyyy-MM-dd"),
                    Requests = requests,
                    AvailableCapacity = availableCapacity
                });
            }

            return report;
        }
    }
}
