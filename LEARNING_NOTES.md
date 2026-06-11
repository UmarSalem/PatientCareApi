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

## Feature 04: Infrastructure Persistence With EF Core

### What Was Created

- EF Core DbContext:
  - `PatientCareDbContext`
- Repository implementations:
  - `PatientRepository`
  - `TreatmentRepository`
  - `AppointmentRepository`
- Unit of Work implementation:
  - `UnitOfWork`
- EF Core SQLite package reference in Infrastructure

### Why It Was Created

The Application layer defines repository interfaces and `IUnitOfWork`. This feature adds the Infrastructure classes that implement those abstractions using EF Core.

This keeps EF Core out of the Domain and Application layers.

Repositories query and stage data changes. `UnitOfWork` commits those changes by calling `DbContext.SaveChangesAsync`.

### Files Changed

- `PatientCareApi.Infrastructure/PatientCareApi.Infrastructure.csproj`
- `PatientCareApi.Infrastructure/Data/PatientCareDbContext.cs`
- `PatientCareApi.Infrastructure/Repositories/PatientRepository.cs`
- `PatientCareApi.Infrastructure/Repositories/TreatmentRepository.cs`
- `PatientCareApi.Infrastructure/Repositories/AppointmentRepository.cs`
- `PatientCareApi.Infrastructure/Repositories/UnitOfWork.cs`
- `PatientCareApi.Application/Services/PatientService.cs`
- `README.md`
- `PROJECT_EXPLANATION.md`
- `ARCHITECTURE.md`
- `API_ENDPOINTS.md`
- `LEARNING_NOTES.md`

### How To Test It

Run:

```powershell
dotnet restore --configfile NuGet.Config
dotnet build --no-restore
```

### Repository Implementation Explanation

`PatientRepository` handles patient database operations. `GetAllAsync` uses `AsNoTracking` for read-only performance. `GetByIdAsync` returns a tracked entity because update and delete workflows need EF Core to detect changes.

`TreatmentRepository` handles treatment queries and creation. It filters by `PatientId` in the database instead of loading every treatment into memory.

`AppointmentRepository` handles appointment queries and creation. `GetByIdAsync` returns a tracked appointment so `AppointmentService` can call `Complete()` or `Cancel()` and save the updated status.

`UnitOfWork` owns the final save operation. Repositories do not call `SaveChangesAsync` themselves.

### How To Explain It In An Interview

> I implemented the Infrastructure layer with EF Core. The Application layer defines repository interfaces, and Infrastructure implements them with `PatientCareDbContext`. Read-only queries use `AsNoTracking` for performance. Update scenarios return tracked entities so domain methods can change state and Unit of Work can save once at the end.

### GitHub Commands For This Feature

```powershell
git stash push -u -m "feature 04 infrastructure persistence"     # Save current uncommitted changes, including new files
git checkout development                                         # Switch to the development branch
git pull origin development                                      # Get latest development code from GitHub
git checkout -B feature/04-infrastructure-persistence            # Create/reset the feature branch from development
git stash pop                                                    # Bring saved Feature 04 changes onto this branch
dotnet restore --configfile NuGet.Config                         # Restore EF Core SQLite package
dotnet build --no-restore                                        # Confirm the project builds
git add .                                                        # Stage all changed files
git commit -m "Add infrastructure persistence"                   # Save the feature as a commit
git push -u origin feature/04-infrastructure-persistence         # Push the feature branch to GitHub
```

Create the pull request:

```text
base: development
compare: feature/04-infrastructure-persistence
```
