namespace MyPharmacy.Api.DTOs.Prescriptions;

public class PrescriptionItemDetailDto
{
    public int MedicationID { get; set; }
    public string MedicationName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
