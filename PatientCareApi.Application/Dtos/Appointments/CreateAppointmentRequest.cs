using System.ComponentModel.DataAnnotations;

namespace PatientCareApi.Application.Dtos.Appointments;

public class CreateAppointmentRequest
{
    [Required]
    public int PatientId { get; init; }

    [Required]
    public DateTime AppointmentDate { get; init; }

    [Required]
    public string Reason { get; init; } = string.Empty;
}
