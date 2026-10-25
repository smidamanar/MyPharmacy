using Microsoft.EntityFrameworkCore;
using MyPharmacy.Api.Data;
using MyPharmacy.Api.Interfaces;
using MyPharmacy.Api.Models;

namespace MyPharmacy.Api.Repositories;

public class PrescriptionRepository : IPrescriptionRepository
{
    private readonly AppDbContext _context;

    public PrescriptionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Prescription?> GetFullPrescriptionAsync(int id)
    {
        return await _context.Prescriptions
            .Include(p => p.Patient)
            .Include(p => p.Pharmacist)
            .Include(p => p.PrescriptionItems!)
                .ThenInclude(pi => pi.Medication)
            .FirstOrDefaultAsync(p => p.PrescriptionID == id);
    }

    public async Task<IEnumerable<Prescription>> GetAllFullAsync()
    {
        return await _context.Prescriptions
            .Include(p => p.Patient)
            .Include(p => p.Pharmacist)
            .Include(p => p.PrescriptionItems!)
                .ThenInclude(pi => pi.Medication)
            .ToListAsync();
    }

    public async Task<Prescription> CreateAsync(Prescription prescription)
    {
        // Important: ne pas tracker les objets Medication
        if (prescription.PrescriptionItems != null)
        {
            foreach (var item in prescription.PrescriptionItems)
                item.Medication = null;
        }

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
        return prescription;
    }

    public async Task<bool> UpdateAsync(Prescription updated)
    {
        var existing = await _context.Prescriptions
            .Include(p => p.PrescriptionItems!)
            .FirstOrDefaultAsync(p => p.PrescriptionID == updated.PrescriptionID);

        if (existing == null)
            return false;

        // Mettre à jour les champs simples
        existing.PatientID = updated.PatientID;
        existing.Notes = updated.Notes;

        // Supprimer les anciens items
        if (existing.PrescriptionItems != null)
            _context.PrescriptionItems.RemoveRange(existing.PrescriptionItems);

        // Ajouter les nouveaux items (sans objet Medication attaché)
        if (updated.PrescriptionItems != null)
        {
            foreach (var item in updated.PrescriptionItems)
                item.Medication = null;

            existing.PrescriptionItems = updated.PrescriptionItems;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var prescription = await _context.Prescriptions.FindAsync(id);
        if (prescription == null)
            return false;

        _context.Prescriptions.Remove(prescription);
        await _context.SaveChangesAsync();
        return true;
    }

}
