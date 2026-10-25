using System;

namespace MyPharmacy.Client.Models.Prescriptions;

public class PrescriptionReadDto
{
    public int PrescriptionID { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
    public int PatientID { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string PharmacistName { get; set; } = string.Empty;
    public List<PrescriptionItemReadDto> Items { get; set; } = new();
}