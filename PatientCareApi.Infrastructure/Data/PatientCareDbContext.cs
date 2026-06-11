using Microsoft.EntityFrameworkCore;
using PatientCareApi.Domain.Entities;
using PatientCareApi.Domain.Enums;

namespace PatientCareApi.Infrastructure.Data;

public class PatientCareDbContext : DbContext
{
    public PatientCareDbContext(DbContextOptions<PatientCareDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Treatment> Treatments => Set<Treatment>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurePatient(modelBuilder);
        ConfigureTreatment(modelBuilder);
        ConfigureAppointment(modelBuilder);
    }

    private static void ConfigurePatient(ModelBuilder modelBuilder)
    {
        var patient = modelBuilder.Entity<Patient>();

        patient.HasKey(entity => entity.Id);

        patient.Property(entity => entity.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        patient.Property(entity => entity.LastName)
            .IsRequired()
            .HasMaxLength(100);

        patient.Property(entity => entity.Email)
            .HasMaxLength(255);

        patient.Property(entity => entity.PhoneNumber)
            .HasMaxLength(50);

        // The Domain exposes read-only collections, so EF writes through the private backing fields.
        patient.Navigation(entity => entity.Treatments)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        patient.Navigation(entity => entity.Appointments)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureTreatment(ModelBuilder modelBuilder)
    {
        var treatment = modelBuilder.Entity<Treatment>();

        treatment.HasKey(entity => entity.Id);

        treatment.Property(entity => entity.Name)
            .IsRequired()
            .HasMaxLength(150);

        treatment.Property(entity => entity.Description)
            .HasMaxLength(500);

        treatment.Property(entity => entity.Notes)
            .HasMaxLength(1000);

        treatment.HasOne(entity => entity.Patient)
            .WithMany(patient => patient.Treatments)
            .HasForeignKey(entity => entity.PatientId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureAppointment(ModelBuilder modelBuilder)
    {
        var appointment = modelBuilder.Entity<Appointment>();

        appointment.HasKey(entity => entity.Id);

        appointment.Property(entity => entity.Reason)
            .IsRequired()
            .HasMaxLength(500);

        appointment.Property(entity => entity.Status)
            .HasConversion(
                status => status.ToString(),
                value => Enum.Parse<AppointmentStatus>(value))
            .HasMaxLength(20);

        appointment.HasOne(entity => entity.Patient)
            .WithMany(patient => patient.Appointments)
            .HasForeignKey(entity => entity.PatientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
