using CarManagementAPI.Contracts;
using System.ComponentModel.DataAnnotations;

namespace CarManagementAPI.DTOs.Maintanace
{
    public class CreateMaintenanceDto
    {
        [Required(ErrorMessage = "Car ID is required")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Garage ID is required")]
        public int GarageId { get; set; }
        [Required(ErrorMessage = "Service type is required")]
        [StringLength(VаlidationConstants.Maintenance.ServiceTypeMaxLength, MinimumLength = VаlidationConstants.Maintenance.ServiceTypeMinLength, ErrorMessage = "Service type must be between {2} and {1} characters")]
        public string ServiceType { get; set; } = null!;

        [Required(ErrorMessage = "Scheduled date is required")]
        public DateTime ScheduledDate { get; set; }
    }
}
