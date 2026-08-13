# API Endpoints

This file tracks the REST endpoints planned for PatientCareApi.

Feature 05 implements these endpoints through controllers. Feature 06 adds global exception handling so service exceptions become clean HTTP error responses.

## Patients

| Method | Endpoint | Purpose |
| --- | --- | --- |
| GET | `/api/patients` | Get all patients |
| GET | `/api/patients/{id}` | Get one patient by id |
| POST | `/api/patients` | Create a patient |
| PUT | `/api/patients/{id}` | Update a patient |
| DELETE | `/api/patients/{id}` | Delete a patient |

## Treatments

| Method | Endpoint | Purpose |
| --- | --- | --- |
| GET | `/api/treatments` | Get all treatments |
| GET | `/api/patients/{id}/treatments` | Get treatments for one patient |
| POST | `/api/patients/{id}/treatments` | Add treatment for one patient |

## Appointments

| Method | Endpoint | Purpose |
| --- | --- | --- |
| GET | `/api/appointments` | Get all appointments |
| GET | `/api/patients/{id}/appointments` | Get appointments for one patient |
| POST | `/api/appointments` | Create an appointment |
| PATCH | `/api/appointments/{id}/complete` | Mark an appointment as completed |
| PATCH | `/api/appointments/{id}/cancel` | Mark an appointment as cancelled |

## Application Use Cases Implemented

- Create, update, get, and delete patients
- Get all treatments and create a treatment for a patient
- Get all appointments and create appointments
- Complete and cancel appointments through domain methods

## Persistence Implemented

- EF Core DbContext for patients, treatments, and appointments
- Repository implementations for Application repository interfaces
- Unit of Work implementation for committing changes
- Initial EF Core migration for the SQLite schema

## DTOs Added

### Patients

- `CreatePatientRequest`
- `UpdatePatientRequest`
- `PatientResponse`

### Treatments

- `CreateTreatmentRequest`
- `TreatmentResponse`

### Appointments

- `PatientsController`
- `TreatmentsController`
- `AppointmentsController`

## Error Responses

| Exception | HTTP Status |
| --- | --- |
| `NotFoundException` | `404 Not Found` |
| `ValidationException` | `400 Bad Request` |
| `ArgumentException` | `400 Bad Request` |
| Unexpected exception | `500 Internal Server Error` |

Example error response:

```json
{
  "statusCode": 404,
  "message": "Patient with id 10 was not found."
}
```

## Integration Test Coverage

Feature 11 adds API integration tests for important endpoint behavior:

- `POST /api/patients` returns `201 Created`
- `GET /api/patients/{id}` returns `404 Not Found` when the patient does not exist
- `PATCH /api/appointments/{id}/complete` changes an appointment status to `Completed`
