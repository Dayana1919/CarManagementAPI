namespace CarManagementAPI.DTOs.Reports
{
    public class GarageDailyAvailabilityReportDto
    {
        public string Date { get; set; } = null!;
        public int Requests { get; set; }
        public int AvailableCapacity { get; set; }
    }
}
