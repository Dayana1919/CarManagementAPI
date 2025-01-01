using CarManagementAPI.Data;
using CarManagementAPI.DTOs.Maintanace;
using CarManagementAPI.DTOs.Reports;

namespace CarManagementAPI.Contracts
{
    public interface IMaintenanceService
    {
       Task<IEnumerable<ResponseMaintenanceDto>> GetFilteredAsync(int? carId, int? garageId, string? startDate, string? endDate);
        Task<ResponseMaintenanceDto> GetByIdAsync(int id);
        Task<int> CreateAsync(Maintenance maintenance);
        Task UpdateAsync(int id, UpdateMaintenanceDto maintenanceDto);
        Task DeleteAsync(Maintenance maintenance);
        Task<IEnumerable<MonthlyRequestsReportDto>> GenerateMonthlyReportAsync(int garageId, string startDate, string endDate);
        

    }
}
