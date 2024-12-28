using CarManagementAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarManagementAPI.Data.Models
{
    public class CarGarage
    {
        public int CarId { get; set; }

        [Required]
        [ForeignKey(nameof(CarId))]
        public Car Car { get; set; } = null!;

        public int GarageId { get; set; }

        [Required]
        [ForeignKey(nameof(GarageId))]
        public Garage Garage { get; set; } = null!;
    }
}
