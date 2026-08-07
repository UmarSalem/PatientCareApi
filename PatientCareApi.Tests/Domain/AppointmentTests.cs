using PatientCareApi.Domain.Entities;
using PatientCareApi.Domain.Enums;

namespace PatientCareApi.Tests.Domain;

public class AppointmentTests
{
    [Fact]
    public void CompletingAppointment_ShouldChangeStatusToCompleted()
    {
        var appointment = new Appointment(
            patientId: 1,
            appointmentDate: DateTime.UtcNow.AddDays(1),
            reason: "Follow-up visit");

        // Status changes go through a domain method instead of setting the enum directly.
        appointment.Complete();

        Assert.Equal(AppointmentStatus.Completed, appointment.Status);
    }
}
