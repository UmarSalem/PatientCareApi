using PatientCareApi.Application.Dtos.Appointments;
using PatientCareApi.Domain.Entities;

namespace PatientCareApi.Application.Mapping;

public static class AppointmentMappingExtensions
{
    public static AppointmentResponse ToResponse(this Appointment appointment)
    {
        return new AppointmentResponse
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            AppointmentDate = appointment.AppointmentDate,
            Reason = appointment.Reason,
            Status = appointment.Status,
            CreatedAt = appointment.CreatedAt,
            UpdatedAt = appointment.UpdatedAt
        };
    }
}
