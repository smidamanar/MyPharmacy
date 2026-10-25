using Microsoft.EntityFrameworkCore;
using MyPharmacy.Api.Models;

namespace MyPharmacy.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Pharmacist> Pharmacists => Set<Pharmacist>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Medication> Medications => Set<Medication>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<PrescriptionItem> PrescriptionItems => Set<PrescriptionItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // IMPORTANT
        base.OnModelCreating(modelBuilder);

        // Decimal precision (fix warning)
        modelBuilder.Entity<Medication>()
            .Property(m => m.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Medication>()
            .Property(m => m.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<Pharmacist>()
            .HasIndex(p => p.Email)
            .IsUnique();

        modelBuilder.Entity<Patient>()
            .HasIndex(p => p.NationalId)
            .IsUnique();

        modelBuilder.Entity<Medication>()
            .ToTable(t => t.HasCheckConstraint("CK_Medication_Price", "[Price] > 0"));

        modelBuilder.Entity<Medication>()
            .ToTable(t => t.HasCheckConstraint("CK_Medication_StockQuantity", "[StockQuantity] >= 0"));

        modelBuilder.Entity<PrescriptionItem>()
            .ToTable(t => t.HasCheckConstraint("CK_PrescriptionItem_Quantity", "[Quantity] > 0"));

        // Composite key PrescriptionItem
        modelBuilder.Entity<PrescriptionItem>()
            .HasKey(pi => new { pi.PrescriptionID, pi.MedicationID });

        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Patient)
            .WithMany(p => p.Prescriptions)
            .HasForeignKey(p => p.PatientID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Pharmacist)
            .WithMany(p => p.Prescriptions)
            .HasForeignKey(p => p.PharmacistID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PrescriptionItem>()
            .HasOne(p => p.Medication)
            .WithMany(m => m.PrescriptionItems)
            .HasForeignKey(p => p.MedicationID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
