using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using MyPharmacy.Client.Models.Prescriptions;

namespace MyPharmacy.Client.Services;

public class PrescriptionService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public PrescriptionService(HttpClient http, ILocalStorageService localStorage)
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

    public async Task<List<PrescriptionReadDto>> GetAllAsync()
    {
        await AddJwtToHeader();
        return await _http.GetFromJsonAsync<List<PrescriptionReadDto>>("api/prescription") ?? new List<PrescriptionReadDto>();
    }

    public async Task<PrescriptionReadDto?> GetByIdAsync(int id)
    {
        await AddJwtToHeader();
        return await _http.GetFromJsonAsync<PrescriptionReadDto>($"api/prescription/{id}");
    }

    public async Task<bool> CreateAsync(PrescriptionCreateDto dto)
    {
        await AddJwtToHeader();
        var res = await _http.PostAsJsonAsync("api/prescription", dto);
        if (!res.IsSuccessStatusCode)
        {
            var msg = await res.Content.ReadAsStringAsync();
            Console.WriteLine($"Create failed: {res.StatusCode} - {msg}");
        }
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(int id, PrescriptionCreateDto dto)
    {
        await AddJwtToHeader();
        var res = await _http.PutAsJsonAsync($"api/prescription/{id}", dto);
        if (!res.IsSuccessStatusCode)
        {
            var msg = await res.Content.ReadAsStringAsync();
            Console.WriteLine($"Update failed: {res.StatusCode} - {msg}");
        }
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await AddJwtToHeader();
        var res = await _http.DeleteAsync($"api/prescription/{id}");
        if (!res.IsSuccessStatusCode)
        {
            var msg = await res.Content.ReadAsStringAsync();
            Console.WriteLine($"Delete failed: {res.StatusCode} - {msg}");
        }
        return res.IsSuccessStatusCode;
    }
}
