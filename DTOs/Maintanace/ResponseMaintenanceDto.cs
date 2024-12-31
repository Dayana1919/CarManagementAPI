namespace CarManagementAPI.DTOs.Maintanace
{
    public class ResponseMaintenanceDto
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string CarName { get; set; } = null!;
        public string ServiceType { get; set; } = null!;
        public DateTime ScheduledDate { get; set; }
        public int GarageId { get; set; }
        public string GarageName { get; set; } = null!;
    }
}
