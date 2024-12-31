namespace CarManagementAPI.DTOs.Garage
{
    public class ResponseGarageDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Capacity { get; set; }
    }
}
