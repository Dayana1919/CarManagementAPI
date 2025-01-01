using CarManagementAPI.Contracts;
using CarManagementAPI.Data;
using CarManagementAPI.DTOs.Maintanace;
using CarManagementAPI.DTOs.Reports;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

public class MaintenanceService : IMaintenanceService
{
    private readonly CarManagementAPIDbContext _context;

    public MaintenanceService(CarManagementAPIDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ResponseMaintenanceDto>> GetFilteredAsync(int? carId, int? garageId, string? startDate, string? endDate)
    {
        var query = _context.Maintenances.Include(m => m.Car).Include(m => m.Garage).AsQueryable();

        if (carId.HasValue)
        {
            query = query.Where(m => m.CarId == carId);
        }

        if (garageId.HasValue)
        {
            query = query.Where(m => m.GarageId == garageId);
        }

        if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out var start))
        {
            query = query.Where(m => m.ScheduledDate >= start);
        }

        if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out var end))
        {
            query = query.Where(m => m.ScheduledDate <= end);
        }

        var maintenances = await query.ToListAsync();
        return maintenances.Select(m => new ResponseMaintenanceDto
        {
            Id = m.Id,
            CarId = m.CarId,
            CarName = m.Car != null ? $"{m.Car.Make} {m.Car.Model}" : "Unknown Car",
            GarageId = m.GarageId,
            GarageName = m.Garage?.Name ?? "Unknown Garage",
            ServiceType = m.ServiceType,
            ScheduledDate = m.ScheduledDate.ToString("yyyy-MM-dd")
        }); ;
    }

    public async Task<ResponseMaintenanceDto> GetByIdAsync(int id)
    {
        var maintenance = await _context.Maintenances
            .Include(m => m.Car)
            .Include(m => m.Garage)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (maintenance == null)
        {
            throw new KeyNotFoundException("Maintenance record not found.");
        }

        return new ResponseMaintenanceDto
        {
            Id = maintenance.Id,
            CarId = maintenance.CarId,
            CarName = maintenance.Car != null ? $"{maintenance.Car.Make} {maintenance.Car.Model}" : "Unknown Car",
            GarageId = maintenance.GarageId,
            GarageName = maintenance.Garage?.Name ?? "Unknown Garage",
            ServiceType = maintenance.ServiceType,
            ScheduledDate = maintenance.ScheduledDate.ToString("yyyy-MM-dd"),
        };
    }

    public async Task<int> CreateAsync(Maintenance maintenanceDto)
    {
        var garage = await _context.Garages.FindAsync(maintenanceDto.GarageId);
        if (garage == null)
        {
            throw new KeyNotFoundException($"Garage with ID {maintenanceDto.GarageId} not found.");
        }

        var car = await _context.Cars.FindAsync(maintenanceDto.CarId);
        if (car == null)
        {
            throw new KeyNotFoundException($"Car with ID {maintenanceDto.CarId} not found.");
        }

        if (_context.Maintenances.Count(m => m.GarageId == maintenanceDto.GarageId && m.ScheduledDate == maintenanceDto.ScheduledDate) >= garage.Capacity)
        {
            throw new InvalidOperationException("Garage is at full capacity for the selected date.");
        }

        var maintenance = new Maintenance
        {
            CarId = maintenanceDto.CarId,
            GarageId = maintenanceDto.GarageId,
            ServiceType = maintenanceDto.ServiceType,
            ScheduledDate = maintenanceDto.ScheduledDate
        };

        _context.Maintenances.Add(maintenance);
        await _context.SaveChangesAsync();

        return maintenance.Id;
    }

    public async Task UpdateAsync(int id, UpdateMaintenanceDto maintenanceDto)
    {
        // Fetch the existing maintenance record
        var maintenance = await _context.Maintenances.FirstOrDefaultAsync(m => m.Id == id);

        if (maintenance == null)
        {
            throw new KeyNotFoundException("Maintenance record not found.");
        }

        // Update the service type and scheduled date
        maintenance.ServiceType = maintenanceDto.ServiceType ?? throw new ArgumentNullException(nameof(maintenanceDto.ServiceType), "ServiceType cannot be null.");
        maintenance.ScheduledDate = maintenanceDto.ScheduledDate;

        // Update the Garage if needed
        if (maintenanceDto.GarageId != maintenance.GarageId)
        {
            var garage = await _context.Garages.FirstOrDefaultAsync(g => g.Id == maintenanceDto.GarageId);
            if (garage == null)
            {
                throw new KeyNotFoundException($"Garage with ID {maintenanceDto.GarageId} not found.");
            }

            maintenance.GarageId = maintenanceDto.GarageId;
        }

        // Update the Car if needed
        if (maintenanceDto.CarId != maintenance.CarId)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == maintenanceDto.CarId);
            if (car == null)
            {
                throw new KeyNotFoundException($"Car with ID {maintenanceDto.CarId} not found.");
            }

            maintenance.CarId = maintenanceDto.CarId;
        }

        // Validate garage capacity
        var existingCount = await _context.Maintenances
            .CountAsync(m => m.GarageId == maintenanceDto.GarageId && m.ScheduledDate == maintenanceDto.ScheduledDate && m.Id != id);

        var garageCapacity = await _context.Garages
            .Where(g => g.Id == maintenanceDto.GarageId)
            .Select(g => g.Capacity)
            .FirstOrDefaultAsync();

        if (existingCount >= garageCapacity)
        {
            throw new InvalidOperationException("The garage is at full capacity for the selected date.");
        }

        // Save changes
        await _context.SaveChangesAsync();
    }


        public async Task DeleteAsync(Maintenance maintenance)
    {
        if (maintenance == null)
        {
            throw new ArgumentNullException(nameof(maintenance), "Maintenance cannot be null.");
        }

        _context.Maintenances.Remove(maintenance);
        await _context.SaveChangesAsync();
    }


    public async Task<IEnumerable<MonthlyRequestsReportDto>> GenerateMonthlyReportAsync(int garageId, string startMonth, string endMonth)
    {
        if (!DateTime.TryParseExact(startMonth, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out var start) ||
            !DateTime.TryParseExact(endMonth, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out var end))
        {
            throw new ArgumentException("Invalid date format for startMonth or endMonth. Use 'yyyy-MM'.");
        }

        // Normalize start and end dates to include the entire month
        start = new DateTime(start.Year, start.Month, 1);
        end = new DateTime(end.Year, end.Month, DateTime.DaysInMonth(end.Year, end.Month));

        // Fetch scheduled dates for maintenance in the specified garage and date range
        var scheduledDates = await _context.Maintenances
            .Where(m => m.GarageId == garageId && m.ScheduledDate >= start && m.ScheduledDate <= end)
            .Select(m => m.ScheduledDate.Date)
            .ToListAsync();

        // Generate all months in the range
        var allMonths = GenerateAllMonths(start, end);

        // Count the requests for each month
        var monthRequestCounts = scheduledDates
            .GroupBy(date => new { date.Year, date.Month })
            .ToDictionary(
                g => $"{g.Key.Year}-{g.Key.Month:D2}",
                g => g.Count()
            );

        // Build the final report, filling in zeroes for months with no requests
        var report = new List<MonthlyRequestsReportDto>();
        foreach (var month in allMonths)
        {
            var requests = monthRequestCounts.ContainsKey(month) ? monthRequestCounts[month] : 0;
            report.Add(new MonthlyRequestsReportDto
            {
                YearMonth = month,
                Requests = requests
            });
        }

        return report;
    }

    private List<string> GenerateAllMonths(DateTime startDate, DateTime endDate)
    {
        var months = new List<string>();

        var current = startDate;
        while (current <= endDate)
        {
            months.Add($"{current.Year}-{current.Month:D2}");
            current = current.AddMonths(1);
        }

        return months;
    }

}