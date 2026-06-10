# API Endpoints

This file tracks the REST endpoints planned for PatientCareApi.

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
