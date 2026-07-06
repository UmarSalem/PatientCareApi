using PatientCareApi.Application.Dtos.Treatments;
using PatientCareApi.Domain.Entities;

namespace PatientCareApi.Application.Mapping;

public static class TreatmentMappingExtensions
{
    public static TreatmentResponse ToResponse(this Treatment treatment)
    {
        return new TreatmentResponse
        {
            Id = treatment.Id,
            PatientId = treatment.PatientId,
            Name = treatment.Name,
            Description = treatment.Description,
            TreatmentDate = treatment.TreatmentDate,
            Notes = treatment.Notes,
            CreatedAt = treatment.CreatedAt
        };
    }
}
