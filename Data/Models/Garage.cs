using CarManagementAPI.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace CarManagementAPI.Models
{
    public class Garage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Location { get; set; } = null!;

        [Required]
        public string? City { get; set; } = null!;

        [Required]
        public int Capacity { get; set; }

        public ICollection<CarGarage> CarsGarage { get; set; } = new HashSet<CarGarage>();
    }
}
