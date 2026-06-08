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
- Documentation files:
  - `README.md`
  - `PROJECT_EXPLANATION.md`
  - `ARCHITECTURE.md`
  - `API_ENDPOINTS.md`
  - `LEARNING_NOTES.md`

### Why It Was Created

The solution structure separates responsibilities:

- Domain contains business rules.
- Application will coordinate use cases.
- Infrastructure will handle database access.
- Api will expose HTTP endpoints.
- Tests will verify behavior.

The domain entities use private setters to protect state. Important state changes, like completing or cancelling an appointment, happen through domain methods.

### Files Changed

- `PatientCareApi.sln`
- `PatientCareApi.Domain/Entities/Patient.cs`
- `PatientCareApi.Domain/Entities/Treatment.cs`
- `PatientCareApi.Domain/Entities/Appointment.cs`
- `PatientCareApi.Domain/Enums/AppointmentStatus.cs`
- `README.md`
- `PROJECT_EXPLANATION.md`
- `ARCHITECTURE.md`
- `API_ENDPOINTS.md`
- `LEARNING_NOTES.md`
- `.gitignore`
- `NuGet.Config`

### How To Test It

At this stage, the main check is that the solution builds:

```powershell
dotnet build
```

Unit tests will be added in a later feature.

### How To Explain It In An Interview

You can say:

> I started by creating a clean architecture solution. The Domain project has no dependency on ASP.NET Core or EF Core. It contains the main healthcare concepts and protects important state with private setters and domain methods.

### GitHub Commands For This Feature

Run these from the project folder:

```powershell
git config --global --add safe.directory C:/Users/admin/source/repos/.Net_Project/PatientCareApi
git init
git branch -M main
git config user.name "UmarSalem"
git config user.email "umars62350@gmail.com"
git remote add origin https://github.com/UmarSalem/PatientCareApi.git
git checkout -b feature/01-solution-and-domain
git status
git add .
git commit -m "Add solution architecture and domain model"
git push -u origin feature/01-solution-and-domain
```
