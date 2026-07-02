using System.Net;
using System.Net.Http.Json;
using PatientCareApi.Application.Dtos.Patients;

namespace PatientCareApi.Tests.Api;

public class PatientEndpointTests
{
    [Fact]
    public async Task CreatePatient_WithValidRequest_ReturnsCreatedPatient()
    {
        using var factory = new PatientCareApiFactory();
        using var client = factory.CreateClient();
        var request = new CreatePatientRequest
        {
            FirstName = "Amina",
            LastName = "Khan",
            DateOfBirth = new DateTime(1991, 4, 12),
            Email = "amina.khan@example.com",
            PhoneNumber = "12345678"
        };

        var response = await client.PostAsJsonAsync("/api/patients", request);
        var patient = await response.Content.ReadFromJsonAsync<PatientResponse>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(patient);
        Assert.True(patient.Id > 0);
        Assert.Equal("Amina", patient.FirstName);
        Assert.Equal("Khan", patient.LastName);
    }

    [Fact]
    public async Task GetPatient_WhenPatientDoesNotExist_ReturnsNotFound()
    {
        using var factory = new PatientCareApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/patients/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
