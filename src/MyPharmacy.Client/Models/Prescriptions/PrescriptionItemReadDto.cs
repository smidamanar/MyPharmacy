using System;
namespace MyPharmacy.Client.Models.Prescriptions;
public class PrescriptionItemReadDto
{
    public int MedicationID { get; set; }
    public string MedicationName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}