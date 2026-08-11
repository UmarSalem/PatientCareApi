using System.Net;
using System.Net.Http.Json;
using PatientCareApi.Application.Dtos.Appointments;
using PatientCareApi.Application.Dtos.Patients;
using PatientCareApi.Domain.Enums;

namespace PatientCareApi.Tests.Api;

public class AppointmentEndpointTests
{
    [Fact]
    public async Task CompleteAppointment_WithExistingAppointment_ReturnsCompletedStatus()
    {
        using var factory = new PatientCareApiFactory();
        using var client = factory.CreateClient();
        var patient = await CreatePatientAsync(client);
        var appointmentRequest = new CreateAppointmentRequest
        {
            PatientId = patient.Id,
            AppointmentDate = DateTime.UtcNow.AddDays(3),
            Reason = "Follow-up visit"
        };

        var createResponse = await client.PostAsJsonAsync("/api/appointments", appointmentRequest);
        var createdAppointment = await createResponse.Content.ReadFromJsonAsync<AppointmentResponse>();

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        Assert.NotNull(createdAppointment);

        // This verifies the full API path: controller, service, repository, EF Core, and domain status method.
        var completeResponse = await client.PatchAsync($"/api/appointments/{createdAppointment.Id}/complete", null);
        var completedAppointment = await completeResponse.Content.ReadFromJsonAsync<AppointmentResponse>();

        Assert.Equal(HttpStatusCode.OK, completeResponse.StatusCode);
        Assert.NotNull(completedAppointment);
        Assert.Equal(AppointmentStatus.Completed, completedAppointment.Status);
    }

    private static async Task<PatientResponse> CreatePatientAsync(HttpClient client)
    {
        var request = new CreatePatientRequest
        {
            FirstName = "Omar",
            LastName = "Saleem",
            DateOfBirth = new DateTime(1988, 2, 20),
            Email = "omar.saleem@example.com"
        };

        var response = await client.PostAsJsonAsync("/api/patients", request);
        var patient = await response.Content.ReadFromJsonAsync<PatientResponse>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(patient);

        return patient;
    }
}
