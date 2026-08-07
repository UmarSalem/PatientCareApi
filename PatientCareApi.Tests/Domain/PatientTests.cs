using PatientCareApi.Domain.Entities;

namespace PatientCareApi.Tests.Domain;

public class PatientTests
{
    [Fact]
    public void CreatingValidPatient_ShouldSucceed()
    {
        var patient = new Patient(
            "Umar",
            "Salem",
            new DateTime(1995, 1, 1),
            "umar@example.com",
            "12345678");

        Assert.Equal("Umar", patient.FirstName);
        Assert.Equal("Salem", patient.LastName);
        Assert.Equal(new DateTime(1995, 1, 1), patient.DateOfBirth);
        Assert.Equal("umar@example.com", patient.Email);
        Assert.Equal("12345678", patient.PhoneNumber);
    }

    [Fact]
    public void CreatingPatientWithEmptyFirstName_ShouldFail()
    {
        // This checks that the Domain entity protects itself from invalid state.
        var exception = Assert.Throws<ArgumentException>(() =>
            new Patient(
                "",
                "Salem",
                new DateTime(1995, 1, 1),
                "umar@example.com",
                "12345678"));

        Assert.Contains("Patient name fields are required", exception.Message);
    }
}
