using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using MyPharmacy.Client.Models.Pharmacists;

namespace MyPharmacy.Client.Services;

public class PharmacistProfileService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public PharmacistProfileService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    private async Task AddJwtToHeader()
    {
        var token = await _localStorage.GetItemAsStringAsync("authToken");
        _http.DefaultRequestHeaders.Authorization = null;
        if (!string.IsNullOrEmpty(token))
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<PharmacistProfileDto?> GetProfileAsync()
    {
        await AddJwtToHeader();
        return await _http.GetFromJsonAsync<PharmacistProfileDto>("api/Pharmacist/me");
    }

    public async Task<bool> UpdateProfileAsync(PharmacistProfileDto dto)
    {
        await AddJwtToHeader();
        var res = await _http.PutAsJsonAsync("api/Pharmacist/me", dto);
        if (!res.IsSuccessStatusCode)
        {
            var msg = await res.Content.ReadAsStringAsync();
            Console.WriteLine($"Update profile failed: {res.StatusCode} - {msg}");
        }
        return res.IsSuccessStatusCode;
    }
}
