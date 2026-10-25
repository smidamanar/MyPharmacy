namespace MyPharmacy.Client.Models.Patients;

public class PatientReadDto
{
    public int PatientID { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string? Address { get; set; }
}
