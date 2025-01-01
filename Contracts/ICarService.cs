using CarManagementAPI.DTOs.Car;
using CarManagementAPI.Models;

namespace CarManagementAPI.Contracts
{
    public interface ICarService
    {
        Task<IEnumerable<ResponseCarDto>> GetAllAsync();
        Task<ResponseCarDto> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateCarDto carDto);
        Task UpdateAsync(int id, UpdateCarDto carDto);
        Task DeleteAsync(Car car);
        Task<IEnumerable<ResponseCarDto>> GetFilteredAsync(string? make, int? fromYear, int? toYear, int? garageId);
    }
}
