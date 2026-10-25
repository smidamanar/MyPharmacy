namespace MyPharmacy.Api.DTOs.Dashboard;

public sealed class DashboardStatsDto
{
    public int TotalPatients { get; init; }
    public int TotalMedications { get; init; }
    public int TotalPrescriptions { get; init; }
    public int TodayPrescriptions { get; init; }
    public int LowStockMedications { get; init; }
}
