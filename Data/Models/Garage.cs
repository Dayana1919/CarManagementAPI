using CarManagementAPI.Contracts;
using System.ComponentModel.DataAnnotations;

namespace CarManagementAPI.Models
{
    public class Garage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(VаlidationConstants.Garage.NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(VаlidationConstants.Garage.LocationMaxLength)]
        public string Location { get; set; } = null!;

        [Required]
        [StringLength(VаlidationConstants.Garage.CityMaxLength)]
        public string City { get; set; } = null!;

        [Required]
        [Range(VаlidationConstants.Garage.CapacityMinValue, int.MaxValue)]
        public int Capacity { get; set; }
    }
}
