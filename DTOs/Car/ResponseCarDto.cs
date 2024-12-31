using CarManagementAPI.DTOs.Garage;

namespace CarManagementAPI.DTOs.Car
{
    public class ResponseCarDto
    {
        public int Id { get; set; }
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int ProductionYear { get; set; }
        public string LicensePlate { get; set; } = null!;
        public List<ResponseGarageDto> Garages { get; set; } = new();
    }
}
