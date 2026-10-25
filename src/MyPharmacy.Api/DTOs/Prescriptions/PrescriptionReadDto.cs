namespace MyPharmacy.Api.DTOs.Prescriptions;

public class PrescriptionReadDto
{
    public int PrescriptionID { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }

    public int PatientID { get; set; }
    public string PatientName { get; set; } = string.Empty;

    public List<PrescriptionItemDetailDto> Items { get; set; } = new();
    public string PharmacistName { get; set; } = string.Empty;

}
