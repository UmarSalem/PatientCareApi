namespace PatientCareApi.Domain.Entities;

public class Patient
{
    private readonly List<Treatment> _treatments = [];
    private readonly List<Appointment> _appointments = [];

    private Patient()
    {
    }

    public Patient(string firstName, string lastName, DateTime dateOfBirth, string? email, string? phoneNumber)
    {
        ValidateRequiredName(firstName, nameof(firstName));
        ValidateRequiredName(lastName, nameof(lastName));

        if (dateOfBirth.Date >= DateTime.UtcNow.Date)
        {
            throw new ArgumentException("Date of birth must be in the past.", nameof(dateOfBirth));
        }

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        DateOfBirth = dateOfBirth.Date;
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
        PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public int Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateTime DateOfBirth { get; private set; }
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public IReadOnlyCollection<Treatment> Treatments => _treatments.AsReadOnly();
    public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

    public void Update(string firstName, string lastName, DateTime dateOfBirth, string? email, string? phoneNumber)
    {
        ValidateRequiredName(firstName, nameof(firstName));
        ValidateRequiredName(lastName, nameof(lastName));

        if (dateOfBirth.Date >= DateTime.UtcNow.Date)
        {
            throw new ArgumentException("Date of birth must be in the past.", nameof(dateOfBirth));
        }

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        DateOfBirth = dateOfBirth.Date;
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
        PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateRequiredName(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Patient name fields are required.", parameterName);
        }
    }
}
