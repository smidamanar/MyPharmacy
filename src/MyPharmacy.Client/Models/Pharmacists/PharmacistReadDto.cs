namespace MyPharmacy.Client.Models.Pharmacists
{
    public class PharmacistReadDto
    {
        public int PharmacistID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
    }
}
