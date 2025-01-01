using CarManagementAPI.DTOs.Garage;
using CarManagementAPI.Models;

namespace CarManagementAPI.Contracts
{
    public interface IGarageService
    {
        Task<IEnumerable<ResponseGarageDto>> GetAllAsync();
        Task<ResponseGarageDto> GetByIdAsync(int id);
        Task<int> CreateAsync(Garage garage);
        Task UpdateAsync(int id, UpdateGarageDto garageDto);
        Task DeleteAsync(Garage garage);
        Task<IEnumerable<ResponseGarageDto>> GetFilteredAsync(string? city);
        Task<IEnumerable<DTOs.Reports.GarageDailyAvailabilityReportDto>> GenerateDailyAvailabilityReportAsync(int garageId, string startDate, string endDate);

    }
}
