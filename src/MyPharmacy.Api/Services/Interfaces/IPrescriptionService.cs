using MyPharmacy.Api.Models;

using MyPharmacy.Api.DTOs.Prescriptions;

namespace MyPharmacy.Api.Services.Interfaces;

public interface IPrescriptionService
{
    Task<Prescription?> GetFullPrescriptionAsync(int id);
    Task<IEnumerable<Prescription>> GetAllFullAsync(int? pharmacistId = null);
    Task<Prescription> CreateAsync(PrescriptionCreateDto request, int pharmacistId);
    Task UpdateAsync(int id, PrescriptionCreateDto request, int pharmacistId, bool isAdmin);
    Task DeleteAsync(int id, int pharmacistId, bool isAdmin);
}
