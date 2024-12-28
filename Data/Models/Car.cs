using CarManagementAPI.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace CarManagementAPI.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Make { get; set; } = null!;

        [Required]
        public string Model { get; set; } = null!;

        [Required]
        public int ProductionYear { get; set; }

        [Required]
        public string LicensePlate { get; set; } = null!;
        public ICollection<CarGarage> CarGarages { get; set; } = new HashSet<CarGarage>();
    }
}
