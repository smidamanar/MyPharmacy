namespace MyPharmacy.Client.Models.Medications;

public class MedicationReadDto
{
    public int MedicationID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public decimal Price { get; set; }
}
