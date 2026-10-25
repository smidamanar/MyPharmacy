namespace MyPharmacy.Client.Models.Patients;
public class PatientCreateDto
{
    public string FullName { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Address { get; set; } = string.Empty;
}
