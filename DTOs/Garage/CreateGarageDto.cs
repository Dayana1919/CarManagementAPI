using CarManagementAPI.Contracts;
using System.ComponentModel.DataAnnotations;

namespace CarManagementAPI.DTOs.Garage
{
    public class CreateGarageDto
    {
        [Required(ErrorMessage = "Garage name is required")]
        [StringLength(VаlidationConstants.Garage.NameMaxLength, ErrorMessage = "Name cannot exceed {1} characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Location is required")]
        [StringLength(VаlidationConstants.Garage.LocationMaxLength, ErrorMessage = "Location cannot exceed {1} characters")]
        public string Location { get; set; } = null!;

        [Required(ErrorMessage = "City is required")]
        [StringLength(VаlidationConstants.Garage.CityMaxLength, ErrorMessage = "City cannot exceed {1} characters")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Capacity is required")]
        [Range(VаlidationConstants.Garage.CapacityMinValue, int.MaxValue, ErrorMessage = "Capacity must be at least {1}")]
        public int Capacity { get; set; }
    }
}
