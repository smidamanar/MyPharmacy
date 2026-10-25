namespace MyPharmacy.Client.Models.Auth
{
    public class RegisterRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // frontend only
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
