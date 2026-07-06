using System.ComponentModel.DataAnnotations;

namespace PatientCareApi.Application.Validation;

public static class RequestValidator
{
    // Services call this so validation is not hidden inside controllers only.
    public static void Validate(object request)
    {
        var context = new ValidationContext(request);
        var results = new List<ValidationResult>();

        if (Validator.TryValidateObject(request, context, results, validateAllProperties: true))
        {
            return;
        }

        var message = string.Join(" ", results.Select(result => result.ErrorMessage));
        throw new ValidationException(message);
    }

    public static void EnsureDateOfBirthIsInPast(DateTime dateOfBirth)
    {
        if (dateOfBirth.Date >= DateTime.UtcNow.Date)
        {
            throw new ValidationException("Date of birth must be in the past.");
        }
    }
}
