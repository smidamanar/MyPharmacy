using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyPharmacy.Client.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace MyPharmacy.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
                ?? throw new InvalidOperationException("ApiBaseUrl is required in wwwroot/appsettings.json.");
            builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<PharmacistService>();
            builder.Services.AddScoped<MedicationService>();
            builder.Services.AddScoped<PatientService>();
            builder.Services.AddScoped<PrescriptionService>();
            builder.Services.AddScoped<DashboardService>();
            builder.Services.AddScoped<PharmacistProfileService>();
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddScoped<CustomAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
                provider.GetRequiredService<CustomAuthStateProvider>());

            builder.Services.AddAuthorizationCore();

            await builder.Build().RunAsync();
        }
    }
}
