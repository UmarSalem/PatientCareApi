using PatientCareApi.Domain.Enums;

namespace PatientCareApi.Application.Dtos.Appointments;

public class AppointmentResponse
{
    public int Id { get; init; }
    public int PatientId { get; init; }
    public DateTime AppointmentDate { get; init; }
    public string Reason { get; init; } = string.Empty;
    public AppointmentStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
