using CarManagementAPI.Contracts;
using CarManagementAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarManagementAPI.Data
{
    public class Maintenance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CarId { get; set; }

        [ForeignKey("CarId")]
        public Car? Car { get; set; }

        [Required]
        public int GarageId { get; set; }

        [ForeignKey("GarageId")]
        public Garage? Garage { get; set; }
        [Required]
        [StringLength(VаlidationConstants.Maintenance.ServiceTypeMaxLength, MinimumLength = VаlidationConstants.Maintenance.ServiceTypeMinLength)]
        public string ServiceType { get; set; } = null!;

        [Required]
        public DateTime ScheduledDate { get; set; }
    }
}
