using MyPharmacy.Api.Interfaces;
using MyPharmacy.Api.Models;
using MyPharmacy.Api.Services.Interfaces;

using System.Data;
using Microsoft.EntityFrameworkCore;
using MyPharmacy.Api.Data;
using MyPharmacy.Api.DTOs.Prescriptions;

namespace MyPharmacy.Api.Services;

public class PrescriptionService(AppDbContext db) : IPrescriptionService
{
    public async Task<Prescription?> GetFullPrescriptionAsync(int id) =>
        await FullQuery().FirstOrDefaultAsync(p => p.PrescriptionID == id);

    public async Task<IEnumerable<Prescription>> GetAllFullAsync(int? pharmacistId = null)
    {
        var query = FullQuery();
        if (pharmacistId.HasValue)
            query = query.Where(p => p.PharmacistID == pharmacistId.Value);
        return await query.OrderByDescending(p => p.Date).ToListAsync();
    }

    public async Task<Prescription> CreateAsync(PrescriptionCreateDto request, int pharmacistId)
    {
        var items = NormalizeItems(request);
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        await EnsurePatientExists(request.PatientID);
        var medications = await LoadAndValidateMedicationStock(items, new Dictionary<int, int>());
        var prescription = new Prescription
        {
            Date = DateTime.UtcNow,
            PatientID = request.PatientID,
            PharmacistID = pharmacistId,
            Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim(),
            PrescriptionItems = items.Select(i => new PrescriptionItem { MedicationID = i.MedicationID, Quantity = i.Quantity }).ToList()
        };
        foreach (var item in items)
            medications[item.MedicationID].StockQuantity -= item.Quantity;
        db.Prescriptions.Add(prescription);
        await SaveAndCommit(transaction);
        return prescription;
    }

    public async Task UpdateAsync(int id, PrescriptionCreateDto request, int pharmacistId, bool isAdmin)
    {
        var items = NormalizeItems(request);
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var existing = await FullQuery().FirstOrDefaultAsync(p => p.PrescriptionID == id)
            ?? throw new ResourceNotFoundException("Prescription not found.");
        EnsureOwnerOrAdmin(existing, pharmacistId, isAdmin);
        await EnsurePatientExists(request.PatientID);
        var oldQuantities = existing.PrescriptionItems!.ToDictionary(i => i.MedicationID, i => i.Quantity);
        var medications = await LoadAndValidateMedicationStock(items, oldQuantities);
        foreach (var medicationId in oldQuantities.Keys.Union(items.Select(i => i.MedicationID)))
        {
            var newQuantity = items.FirstOrDefault(i => i.MedicationID == medicationId)?.Quantity ?? 0;
            medications[medicationId].StockQuantity -= newQuantity - oldQuantities.GetValueOrDefault(medicationId);
        }
        existing.PatientID = request.PatientID;
        existing.Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim();
        db.PrescriptionItems.RemoveRange(existing.PrescriptionItems!);
        existing.PrescriptionItems = items.Select(i => new PrescriptionItem { PrescriptionID = id, MedicationID = i.MedicationID, Quantity = i.Quantity }).ToList();
        await SaveAndCommit(transaction);
    }

    public async Task DeleteAsync(int id, int pharmacistId, bool isAdmin)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var prescription = await FullQuery().FirstOrDefaultAsync(p => p.PrescriptionID == id)
            ?? throw new ResourceNotFoundException("Prescription not found.");
        EnsureOwnerOrAdmin(prescription, pharmacistId, isAdmin);
        foreach (var item in prescription.PrescriptionItems!)
            item.Medication!.StockQuantity += item.Quantity;
        db.Prescriptions.Remove(prescription);
        await SaveAndCommit(transaction);
    }

    private IQueryable<Prescription> FullQuery() => db.Prescriptions.Include(p => p.Patient).Include(p => p.Pharmacist)
        .Include(p => p.PrescriptionItems!).ThenInclude(i => i.Medication);

    private async Task EnsurePatientExists(int patientId)
    {
        if (!await db.Patients.AnyAsync(p => p.PatientID == patientId))
            throw new DomainValidationException("Patient not found.");
    }

    private async Task<Dictionary<int, Medication>> LoadAndValidateMedicationStock(IReadOnlyCollection<PrescriptionItemCreateDto> items, IReadOnlyDictionary<int, int> oldQuantities)
    {
        var ids = items.Select(i => i.MedicationID).Union(oldQuantities.Keys).ToList();
        var medications = await db.Medications.Where(m => ids.Contains(m.MedicationID)).ToDictionaryAsync(m => m.MedicationID);
        if (medications.Count != ids.Count)
            throw new DomainValidationException("One or more medications no longer exist.");
        foreach (var item in items)
        {
            var medication = medications[item.MedicationID];
            if (medication.StockQuantity + oldQuantities.GetValueOrDefault(item.MedicationID) < item.Quantity)
                throw new DomainValidationException($"Insufficient stock for {medication.Name}.");
        }
        return medications;
    }

    private static List<PrescriptionItemCreateDto> NormalizeItems(PrescriptionCreateDto request)
    {
        var items = request.Items.GroupBy(i => i.MedicationID).Select(g => new PrescriptionItemCreateDto { MedicationID = g.Key, Quantity = g.Sum(i => i.Quantity) }).ToList();
        if (items.Count == 0) throw new DomainValidationException("At least one medication is required.");
        return items;
    }

    private static void EnsureOwnerOrAdmin(Prescription prescription, int pharmacistId, bool isAdmin)
    {
        if (!isAdmin && prescription.PharmacistID != pharmacistId)
            throw new DomainValidationException("You can only modify prescriptions that you created.");
    }

    private async Task SaveAndCommit(Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction)
    {
        try { await db.SaveChangesAsync(); await transaction.CommitAsync(); }
        catch (DbUpdateConcurrencyException) { throw new InventoryConcurrencyException(); }
    }
}
