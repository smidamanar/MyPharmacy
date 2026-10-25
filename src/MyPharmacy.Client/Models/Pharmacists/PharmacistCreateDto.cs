namespace MyPharmacy.Client.Models.Pharmacists
{
    public class PharmacistCreateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // for creation
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string Role { get; set; } = "Pharmacist"; // default role
    }
}
