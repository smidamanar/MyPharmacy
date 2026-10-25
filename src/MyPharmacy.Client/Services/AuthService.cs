using System.Net.Http.Json;
using MyPharmacy.Client.Models.Auth;

namespace MyPharmacy.Client.Services;

public class AuthService
{
    private readonly HttpClient _http;

    public AuthService(HttpClient http)
    {
        _http = http;
    }

    public async Task<string?> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/Auth/login", request);


            if (!response.IsSuccessStatusCode)
                return null; // Invalid credentials

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result?.Token;
        }
        catch
        {
            return null;
        }
    }

}
