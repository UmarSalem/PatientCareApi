# PatientCareApi

PatientCareApi is an ASP.NET Core Web API practice project for managing patients, treatments, and appointments in a healthcare-style system.

The project is being built feature by feature so it can be demonstrated as both a coding challenge solution and a GitHub workflow practice project.

## Technologies

- .NET 9
- ASP.NET Core Web API
- Clean layered architecture
- EF Core with SQLite
- Swagger/OpenAPI
- xUnit
- GitHub Actions CI/CD
- Docker, planned for a later feature

## Current Status

Completed so far:

- Feature 01: solution structure and Domain model
- Feature 02: DTOs, repository interfaces, service interfaces, and Unit of Work contract
- Feature 03: Application services, validation helper, DTO mapping, and custom not-found exception
- Feature 04: EF Core DbContext, repository implementations, and Unit of Work implementation
- Feature 05: API dependency injection, Swagger UI, connection string, and REST controllers
- Feature 06: Global error handling middleware

## How To Run

The API can run after packages are restored and the database is created through EF Core migrations.

```powershell
dotnet restore --configfile NuGet.Config
dotnet run --project PatientCareApi.Api
```

## How To Run Tests

Tests will be added in a later feature.

```powershell
dotnet test
```

## Swagger

Swagger/OpenAPI will be available when the API feature is completed:

```text
https://localhost:<port>/swagger
```

## Main Endpoint Plan

- `GET /api/patients`
- `GET /api/patients/{id}`
- `POST /api/patients`
- `PUT /api/patients/{id}`
- `DELETE /api/patients/{id}`
- `GET /api/treatments`
- `GET /api/patients/{id}/treatments`
- `POST /api/patients/{id}/treatments`
- `GET /api/appointments`
- `GET /api/patients/{id}/appointments`
- `POST /api/appointments`
- `PATCH /api/appointments/{id}/complete`
- `PATCH /api/appointments/{id}/cancel`

## GitHub Workflow

Each feature should be committed on its own branch and pushed to GitHub.

For Feature 06:

```powershell
git stash push -u -m "feature 06 error handling"
git checkout development
git pull origin development
git checkout -B feature/06-error-handling
git stash pop
dotnet restore --configfile NuGet.Config
dotnet build --no-restore
git add .
git commit -m "Add global error handling middleware"
git push -u origin feature/06-error-handling
```

See `GIT_WORKFLOW.md` for merge vs rebase notes.
