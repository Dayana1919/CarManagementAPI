using CarManagementAPI.Contracts;
using System.ComponentModel.DataAnnotations;

namespace CarManagementAPI.DTOs.Car
{
    public class CreateCarDto
    {
        [Required(ErrorMessage = "Make is required")]
        [StringLength(VаlidationConstants.Car.MakeMaxLength, ErrorMessage = "Make cannot exceed {1} characters")]
        public string Make { get; set; } = null!;

        [Required(ErrorMessage = "Model is required")]
        [StringLength(VаlidationConstants.Car.ModelMaxLength, ErrorMessage = "Model cannot exceed {1} characters")]
        public string Model { get; set; } = null!;

        [Required(ErrorMessage = "Production year is required")]
        [Range(VаlidationConstants.Car.ProductionYearMin, 2100, ErrorMessage = "Production year must be between {1} and {2}")]
        public int ProductionYear { get; set; }

        [Required(ErrorMessage = "License plate is required")]
        [StringLength(VаlidationConstants.Car.LicensePlateMaxLength, ErrorMessage = "License plate cannot exceed {1} characters")]
        public string LicensePlate { get; set; } = null!;

        public List<int> GarageIds { get; set; } = new();
    }
}
