namespace CarManagementAPI.DTOs.Reports
{
    public class MonthlyRequestsReportDto
    {
        public string YearMonth { get; set; } = null!;
        public int Requests { get; set; }
    }
}
