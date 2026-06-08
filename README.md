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

Feature 01 creates the solution structure and the Domain model.

Created projects:

- `PatientCareApi.Domain`
- `PatientCareApi.Application`
- `PatientCareApi.Infrastructure`
- `PatientCareApi.Api`
- `PatientCareApi.Tests`

## How To Run

The runnable API will be completed in a later feature after services, repositories, EF Core, and controllers are added.

Later, the command will be:

```powershell
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

For this first feature:

```powershell
git checkout -b feature/01-solution-and-domain
git add .
git commit -m "Add solution architecture and domain model"
git push -u origin feature/01-solution-and-domain
```
