using System.ComponentModel.DataAnnotations;

namespace PatientCareApi.Application.Dtos.Treatments;

public class CreateTreatmentRequest
{
    [Required]
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    [Required]
    public DateTime TreatmentDate { get; init; }

    public string? Notes { get; init; }
}
