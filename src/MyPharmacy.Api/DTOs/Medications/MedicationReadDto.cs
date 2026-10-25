namespace MyPharmacy.Api.DTOs.Medications;

public class MedicationReadDto
{
    public int MedicationID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}
