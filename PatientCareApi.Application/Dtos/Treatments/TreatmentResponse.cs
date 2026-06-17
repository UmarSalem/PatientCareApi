namespace PatientCareApi.Application.Dtos.Treatments;

public class TreatmentResponse
{
    public int Id { get; init; }
    public int PatientId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime TreatmentDate { get; init; }
    public string? Notes { get; init; }
    public DateTime CreatedAt { get; init; }
}
