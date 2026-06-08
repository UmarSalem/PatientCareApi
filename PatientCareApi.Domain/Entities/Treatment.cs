namespace PatientCareApi.Domain.Entities;

public class Treatment
{
    private Treatment()
    {
    }

    public Treatment(int patientId, string name, string? description, DateTime treatmentDate, string? notes)
    {
        if (patientId <= 0)
        {
            throw new ArgumentException("Treatment must belong to a patient.", nameof(patientId));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Treatment name is required.", nameof(name));
        }

        PatientId = patientId;
        Name = name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        TreatmentDate = treatmentDate;
        Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        CreatedAt = DateTime.UtcNow;
    }

    public int Id { get; private set; }
    public int PatientId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime TreatmentDate { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Patient Patient { get; private set; } = null!;
}
