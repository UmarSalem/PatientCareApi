# Learning Notes

This file is updated after each major feature so the project can be explained step by step.

## Feature 01: Solution Architecture And Domain Model

### What Was Created

- A .NET solution named `PatientCareApi`
- Five projects:
  - `PatientCareApi.Domain`
  - `PatientCareApi.Application`
  - `PatientCareApi.Infrastructure`
  - `PatientCareApi.Api`
  - `PatientCareApi.Tests`
- Domain entities:
  - `Patient`
  - `Treatment`
  - `Appointment`
- Domain enum:
  - `AppointmentStatus`

### Why It Was Created

The solution structure separates responsibilities:

- Domain contains business rules.
- Application coordinates use cases.
- Infrastructure handles database access.
- Api exposes HTTP endpoints.
- Tests verify behavior.

The domain entities use private setters to protect state. Important state changes, like completing or cancelling an appointment, happen through domain methods.

### How To Explain It In An Interview

> I started by creating a clean architecture solution. The Domain project has no dependency on ASP.NET Core or EF Core. It contains the main healthcare concepts and protects important state with private setters and domain methods.

## Feature 02: Application DTOs And Contracts

### What Was Created

- Patient DTOs:
  - `CreatePatientRequest`
  - `UpdatePatientRequest`
  - `PatientResponse`
- Treatment DTOs:
  - `CreateTreatmentRequest`
  - `TreatmentResponse`
- Appointment DTOs:
  - `CreateAppointmentRequest`
  - `AppointmentResponse`
- Repository interfaces:
  - `IPatientRepository`
  - `ITreatmentRepository`
  - `IAppointmentRepository`
  - `IUnitOfWork`
- Service interfaces:
  - `IPatientService`
  - `ITreatmentService`
  - `IAppointmentService`

### Why It Was Created

DTOs define what data enters and leaves the API. Repository interfaces define what data operations the service layer needs. `IUnitOfWork` defines the save operation. Service interfaces define the use cases that controllers will call.

### How To Explain It In An Interview

> I added the Application layer contracts before writing controllers or database code. The DTOs shape API input and output. The service interfaces describe use cases, the repository interfaces describe persistence needs, and Unit of Work describes saving changes.

## Feature 03: Application Service Implementations

### What Was Created

- Custom exception:
  - `NotFoundException`
- Validation helper:
  - `RequestValidator`
- Mapping extension classes:
  - `PatientMappingExtensions`
  - `TreatmentMappingExtensions`
  - `AppointmentMappingExtensions`
- Service implementations:
  - `PatientService`
  - `TreatmentService`
  - `AppointmentService`

### Why It Was Created

This feature adds the real Application-layer workflows.

The services validate request DTOs, check that related records exist, create or update domain entities, call repository interfaces, save through `IUnitOfWork`, and return response DTOs.

This keeps controllers thin because controllers will only call services and return HTTP responses.

Using `IUnitOfWork` also keeps saving separate from repository methods. Repositories describe data access, and Unit of Work describes committing changes.

### Files Changed

- `PatientCareApi.Application/Dtos`
- `PatientCareApi.Application/Repositories`
- `PatientCareApi.Application/Services`
- `PatientCareApi.Application/Exceptions/NotFoundException.cs`
- `PatientCareApi.Application/Validation/RequestValidator.cs`
- `PatientCareApi.Application/Mapping`
- `README.md`
- `PROJECT_EXPLANATION.md`
- `ARCHITECTURE.md`
- `API_ENDPOINTS.md`
- `LEARNING_NOTES.md`

### How To Test It

Run:

```powershell
dotnet build --no-restore
```

### How To Explain It In An Interview

> I implemented the Application services as the use-case layer. The services validate DTOs, check for missing records, use repositories through interfaces, call domain methods for important behavior, save through Unit of Work, and return response DTOs. For example, completing an appointment loads the appointment, calls `appointment.Complete()`, saves changes through `IUnitOfWork`, and maps the result to `AppointmentResponse`.

### GitHub Commands For This Feature

If Feature 02 is already merged into `development`, use:

```powershell
git add .
git commit -m "Add application service implementations"
git push -u origin feature/03-application-services
```

If Feature 02 is not merged into `development`, create the Feature 03 PR against Feature 02:

```text
base: feature/02-application-contracts
compare: feature/03-application-services
```

## Feature 02: Application DTOs And Contracts

### What Was Created

- Patient DTOs:
  - `CreatePatientRequest`
  - `UpdatePatientRequest`
  - `PatientResponse`
- Treatment DTOs:
  - `CreateTreatmentRequest`
  - `TreatmentResponse`
- Appointment DTOs:
  - `CreateAppointmentRequest`
  - `AppointmentResponse`
- Repository interfaces:
  - `IPatientRepository`
  - `ITreatmentRepository`
  - `IAppointmentRepository`
- Service interfaces:
  - `IPatientService`
  - `ITreatmentService`
  - `IAppointmentService`

### Why It Was Created

This feature creates the Application layer contracts.

DTOs define what data enters and leaves the API. Repository interfaces define what data operations the service layer needs. Service interfaces define the use cases that controllers will call.

This keeps the API controllers thin and keeps EF Core details outside the Application layer.

### Files Changed

- `PatientCareApi.Application/Dtos/Patients/CreatePatientRequest.cs`
- `PatientCareApi.Application/Dtos/Patients/UpdatePatientRequest.cs`
- `PatientCareApi.Application/Dtos/Patients/PatientResponse.cs`
- `PatientCareApi.Application/Dtos/Treatments/CreateTreatmentRequest.cs`
- `PatientCareApi.Application/Dtos/Treatments/TreatmentResponse.cs`
- `PatientCareApi.Application/Dtos/Appointments/CreateAppointmentRequest.cs`
- `PatientCareApi.Application/Dtos/Appointments/AppointmentResponse.cs`
- `PatientCareApi.Application/Repositories/IPatientRepository.cs`
- `PatientCareApi.Application/Repositories/ITreatmentRepository.cs`
- `PatientCareApi.Application/Repositories/IAppointmentRepository.cs`
- `PatientCareApi.Application/Services/IPatientService.cs`
- `PatientCareApi.Application/Services/ITreatmentService.cs`
- `PatientCareApi.Application/Services/IAppointmentService.cs`
- `README.md`
- `PROJECT_EXPLANATION.md`
- `ARCHITECTURE.md`
- `API_ENDPOINTS.md`
- `LEARNING_NOTES.md`

### How To Test It

Run:

```powershell
dotnet build
```

This confirms the Application project still compiles and the dependency rule is respected: Application depends only on Domain.

### How To Explain It In An Interview

You can say:

> I added the Application layer contracts before writing controllers or database code. The DTOs shape API input and output. The service interfaces describe use cases, and the repository interfaces describe persistence needs. This keeps the controller layer thin and keeps EF Core out of the Application layer.

### GitHub Commands For This Feature

Use this branch flow:

```powershell
git checkout development
git pull origin development
git checkout -b feature/02-application-contracts
git status
git add .
git commit -m "Add application DTOs and contracts"
git push -u origin feature/02-application-contracts
```

Create the pull request:

```text
base: development
compare: feature/02-application-contracts
```
