namespace MyPharmacy.Client.Models.Dashboard;

public class DashboardStatsDto
{
    public int TotalPatients { get; set; }
    public int TotalMedications { get; set; }
    public int TotalPrescriptions { get; set; }
    public int TodayPrescriptions { get; set; }
    public int LowStockMedications { get; set; }
}
