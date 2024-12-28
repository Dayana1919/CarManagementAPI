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
        public Car Car { get; set; } = null!;

        [Required]
        public int GarageId { get; set; }

        [ForeignKey("GarageId")]
        public Garage Garage { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string ServiceType { get; set; } = null!;

        [Required]
        public DateTime ScheduledDate { get; set; } 
    }
}
