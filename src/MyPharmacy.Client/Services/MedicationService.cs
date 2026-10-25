using System.Net.Http.Json;
using MyPharmacy.Client.Models.Medications;
using Blazored.LocalStorage;

namespace MyPharmacy.Client.Services;

public class MedicationService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public MedicationService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    private async Task AddJwtToHeader()
    {
        var token = await _localStorage.GetItemAsStringAsync("authToken");
        if (!string.IsNullOrEmpty(token))
        {
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<List<MedicationReadDto>> GetAllAsync()
    {
        await AddJwtToHeader();
        return await _http.GetFromJsonAsync<List<MedicationReadDto>>("api/Medication") ?? new List<MedicationReadDto>();
    }

    public async Task<bool> CreateAsync(MedicationCreateDto dto)
    {
        await AddJwtToHeader();
        var response = await _http.PostAsJsonAsync("api/Medication", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(int id, MedicationCreateDto dto)
    {
        await AddJwtToHeader();
        var response = await _http.PutAsJsonAsync($"api/Medication/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await AddJwtToHeader();
        var response = await _http.DeleteAsync($"api/Medication/{id}");
        return response.IsSuccessStatusCode;
    }
}
