namespace PatientCareApi.Application.Exceptions;

// The API middleware will translate this exception into a 404 Not Found response.
public class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message)
    {
    }
}
