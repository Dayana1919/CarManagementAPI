namespace CarManagementAPI.DTOs.Reports
{
    public class GarageDailyAvailabilityReportDto
    {
        public DateTime Date { get; set; }
        public int Requests { get; set; }
        public int AvailableCapacity { get; set; }
    }
}
