using PatientCareApi.Application.Dtos.Patients;
using PatientCareApi.Domain.Entities;

namespace PatientCareApi.Application.Mapping;

public static class PatientMappingExtensions
{
    // Mapping keeps DTO creation in one place instead of repeating it in every service method.
    public static PatientResponse ToResponse(this Patient patient)
    {
        return new PatientResponse
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            Email = patient.Email,
            PhoneNumber = patient.PhoneNumber,
            CreatedAt = patient.CreatedAt,
            UpdatedAt = patient.UpdatedAt
        };
    }
}
