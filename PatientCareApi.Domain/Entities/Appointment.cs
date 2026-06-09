using PatientCareApi.Domain.Enums;

namespace PatientCareApi.Domain.Entities;

public class Appointment
{
    private Appointment()
    {
    }

    public Appointment(int patientId, DateTime appointmentDate, string reason)
    {
        if (patientId <= 0)
        {
            throw new ArgumentException("Appointment must belong to a patient.", nameof(patientId));
        }

        if (appointmentDate == default)
        {
            throw new ArgumentException("Appointment date is required.", nameof(appointmentDate));
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Appointment reason is required.", nameof(reason));
        }

        PatientId = patientId;
        AppointmentDate = appointmentDate;
        Reason = reason.Trim();
        Status = AppointmentStatus.Scheduled;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public int Id { get; private set; }
    public int PatientId { get; private set; }
    public DateTime AppointmentDate { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public AppointmentStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Patient Patient { get; private set; } = null!;

    // Status changes are domain behavior, so they stay on the entity instead of in controllers.
    public void Complete()
    {
        if (Status == AppointmentStatus.Cancelled)
        {
            throw new ArgumentException("A cancelled appointment cannot be completed.");
        }

        Status = AppointmentStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }

    // The entity protects invalid transitions, such as cancelling an already completed appointment.
    public void Cancel()
    {
        if (Status == AppointmentStatus.Completed)
        {
            throw new ArgumentException("A completed appointment cannot be cancelled.");
        }

        Status = AppointmentStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
}
