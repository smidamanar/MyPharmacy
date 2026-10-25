using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using MyPharmacy.Client.Models.Pharmacists;

namespace MyPharmacy.Client.Services
{
    public class PharmacistService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public PharmacistService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        private async Task AddJwtToHeader()
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            _http.DefaultRequestHeaders.Authorization = null;
            if (!string.IsNullOrEmpty(token))
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<PharmacistReadDto>> GetAllAsync()
        {
            await AddJwtToHeader();
            return await _http.GetFromJsonAsync<List<PharmacistReadDto>>("api/Pharmacist") ?? new List<PharmacistReadDto>();
        }

        public async Task<bool> CreateAsync(PharmacistCreateDto dto)
        {
            await AddJwtToHeader();
            var response = await _http.PostAsJsonAsync("api/Pharmacist", dto);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Create failed: {response.StatusCode} - {content}");
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateAsync(int id, PharmacistCreateDto dto)
        {
            await AddJwtToHeader();
            var response = await _http.PutAsJsonAsync($"api/Pharmacist/{id}", dto);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Update failed: {response.StatusCode} - {content}");
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await AddJwtToHeader();
            var response = await _http.DeleteAsync($"api/Pharmacist/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Delete failed: {response.StatusCode} - {content}");
                return false;
            }
            return true;
        }
    }
}