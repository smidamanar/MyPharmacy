using System.Net.Http.Json;
using Blazored.LocalStorage;
using MyPharmacy.Client.Models.Dashboard;
using System.Net.Http.Headers;

namespace MyPharmacy.Client.Services;

public class DashboardService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public DashboardService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    private async Task AddJwtAsync()
    {
        var token = await _localStorage.GetItemAsStringAsync("authToken");
        if (!string.IsNullOrEmpty(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<DashboardStatsDto> GetStatsAsync()
    {
        await AddJwtAsync();

        return await _http.GetFromJsonAsync<DashboardStatsDto>("api/Dashboard")
            ?? new DashboardStatsDto();
    }
}
