using System.ComponentModel.DataAnnotations;

namespace PatientCareApi.Application.Dtos.Patients;

public class CreatePatientRequest
{
    [Required]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    public string LastName { get; init; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; init; }

    [EmailAddress]
    public string? Email { get; init; }

    public string? PhoneNumber { get; init; }
}
