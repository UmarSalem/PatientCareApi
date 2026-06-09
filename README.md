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

Feature 02 adds the Application contracts.

Completed so far:

- Feature 01: solution structure and Domain model
- Feature 02: DTOs, repository interfaces, and service interfaces

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

For Feature 02:

```powershell
git checkout development
git pull origin development
git checkout -b feature/02-application-contracts
git add .
git commit -m "Add application DTOs and contracts"
git push -u origin feature/02-application-contracts
```
