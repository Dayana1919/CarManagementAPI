using CarManagementAPI.Contracts;
using System.ComponentModel.DataAnnotations;

namespace CarManagementAPI.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(VаlidationConstants.Car.MakeMaxLength)]
        public string Make { get; set; } = null!;

        [Required]
        [StringLength(VаlidationConstants.Car.ModelMaxLength)]
        public string Model { get; set; } = null!;

        [Required]
        public int ProductionYear { get; set; }

        [Required]
        [StringLength(VаlidationConstants.Car.LicensePlateMaxLength)]
        public string LicensePlate { get; set; } = null!;
    }
}
