using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using MyPharmacy.Client.Models.Patients;

namespace MyPharmacy.Client.Services;

public class PatientService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public PatientService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    private async Task AddJwtToHeader()
    {
        var token = await _localStorage.GetItemAsStringAsync("authToken");
        _http.DefaultRequestHeaders.Authorization = null; // important
        if (!string.IsNullOrEmpty(token))
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<List<PatientReadDto>> GetAllAsync()
    {
        await AddJwtToHeader();
        return await _http.GetFromJsonAsync<List<PatientReadDto>>("api/patient") ?? new List<PatientReadDto>();
    }

    public async Task<PatientReadDto?> GetByIdAsync(int id)
    {
        await AddJwtToHeader();
        return await _http.GetFromJsonAsync<PatientReadDto>($"api/patient/{id}");
    }

    public async Task<bool> CreateAsync(PatientCreateDto dto)
    {
        await AddJwtToHeader();
        var response = await _http.PostAsJsonAsync("api/patient", dto);
        if (!response.IsSuccessStatusCode)
        {
            var msg = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Patient create failed: {response.StatusCode} - {msg}");
        }
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(int id, PatientCreateDto dto)
    {
        await AddJwtToHeader();
        var response = await _http.PutAsJsonAsync($"api/patient/{id}", dto);
        if (!response.IsSuccessStatusCode)
        {
            var msg = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Patient update failed: {response.StatusCode} - {msg}");
        }
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await AddJwtToHeader();
        var response = await _http.DeleteAsync($"api/patient/{id}");
        if (!response.IsSuccessStatusCode)
        {
            var msg = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Patient delete failed: {response.StatusCode} - {msg}");
        }
        return response.IsSuccessStatusCode;
    }
}
