# Room Reservation System

A web application for booking meeting rooms in an office. Users can browse rooms, book them for meetings and manage reservations. Every booking is validated in the back end against configurable business rules, and early-ended meetings are tracked so the office can see how rooms are actually used.

Technical assignment for the IDT internship at Present Connection.

## Tech stack

| Part      | Technologies                                                                 |
|-----------|-------------------------------------------------------------------------------|
| Back end  | ASP.NET Core 10 Web API, EF Core InMemory, MediatR 12, OpenAPI + Scalar       |
| Front end | React 19, TypeScript, Vite, Material UI, React Router                         |

## Getting started

Requirements: [.NET 10 SDK](https://dotnet.microsoft.com/download) and [Node.js 20+](https://nodejs.org/).

### Back end

```bash
cd api/src/Presentation
dotnet run
```

The API runs at http://localhost:5158. Interactive API docs (Scalar) are at http://localhost:5158/scalar.
The in-memory database is filled with sample data on every start, including one meeting that is in progress, so "end early" can be tried right away.

### Front end

```bash
cd frontend
npm install
npm run dev
```

Open http://localhost:5173. The Vite dev server forwards `/api` requests to the back end, so no CORS setup is needed.

## Features

- **Rooms** – name, ID, capacity, floor, equipment and status; filter by minimum capacity, floor and equipment (a room must have all selected equipment).
- **Users** – first name, last name, ID and the number of active reservations.
- **Reservations** – room, organizer, title, start/end, attendees and status; filter by room, organizer, status and date range (any reservation that overlaps the range is shown).
- **Actions** – create a reservation, cancel it, or end a meeting early.
- **Validation errors** – rule violations are shown in the UI with their code and message, e.g. `[R2] Room 'Room 4' is already booked for the selected time range.`

## Booking rules

All rules are checked in the back end in `Application/Validators/BookingRulesValidator.cs` before a reservation is saved.

| Code | Rule |
|------|------|
| R1 | A reservation cannot last longer than `MaxDurationDays` (exactly the limit is allowed). |
| R2 | A room cannot have two overlapping reservations. Cancelled reservations do not block the room. |
| R3 | The same organizer cannot have two overlapping meetings, even in different rooms. |
| R4 | The start must be in the future, and the end must be after the start. |
| R5 | The number of attendees cannot exceed the room capacity. |
| R6 | A reservation cannot start more than `MaxBookingHorizonDays` in the future. |
| R7 | Inactive rooms cannot be booked. |

The limits are configured in `api/src/Presentation/appsettings.json`:

```json
"Booking": {
  "MaxDurationDays": 7,
  "MaxBookingHorizonDays": 90
}
```

### Error format

Errors are returned as [ProblemDetails](https://www.rfc-editor.org/rfc/rfc9457) with an extra `code` field:

```json
{
  "title": "Booking rule violation",
  "status": 400,
  "detail": "Room 'Room 1' fits 2 attendees, but the reservation has 3 attendees.",
  "code": "R5"
}
```

| Status | Code | When |
|--------|------|------|
| 400 | `R1`–`R7` | A booking rule is broken |
| 404 | `RESOURCE_NOT_FOUND` | Room, user or reservation does not exist |
| 409 | `INVALID_RESERVATION_STATE` | The action is not possible in the current state (e.g. cancelling a completed reservation) |
| 500 | `INTERNAL_SERVER_ERROR` | Unexpected error (details are logged, not returned) |

## API

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/rooms?minCapacity=&floor=&equipment=` | List rooms with optional filters |
| GET | `/api/users` | List users with their active reservation count |
| GET | `/api/reservations?roomId=&organizerId=&status=&startDate=&endDate=` | List reservations with optional filters |
| POST | `/api/reservations` | Create a reservation (validated against R1–R7) |
| PATCH | `/api/reservations/{id}/cancel` | Cancel an upcoming or ongoing reservation |
| PATCH | `/api/reservations/{id}/end-early` | End an ongoing meeting now |

## Architecture

### Back end – Clean Architecture

```
api/src/
  Domain/          Entities, enums and repository interfaces. No dependencies.
  Application/     Use cases (queries/commands + handlers), DTOs, mappers, booking rules, options. → Domain
  Infrastructure/  EF Core InMemory DbContext, repository implementations, seeders. → Application
  Presentation/    Controllers, exception handling, DI and configuration. → Application, Infrastructure
```

Dependencies only point inwards, so business logic does not depend on EF Core or ASP.NET. Swapping the in-memory database for a real one would only change the Infrastructure layer.

Patterns and decisions:

- **CQRS with MediatR** – each operation is its own query or command with a handler (`UseCases/<Area>/<Operation>/`). Controllers only send requests to `IMediator`, so they stay thin.
- **Repository pattern** – interfaces live in `Domain/IRepositories`, EF Core implementations in `Infrastructure/Repositories`.
- **Dependency injection** – all repositories, the validator and the seeders are registered in the built-in container.
- **Custom mappers** – extension methods (`room.ToDto()`) map entities to DTOs, so the API never exposes entities.
- **Options pattern** – booking limits are bound from `appsettings.json` to `BookingOptions` and injected as `IOptions<BookingOptions>`.
- **Global exception handler** – an `IExceptionHandler` turns exceptions into ProblemDetails responses in one place, so handlers and controllers have no `try/catch`.
- **Database seeders** – one `ISeeder` per entity, run in order (rooms → users → reservations). Dates are relative to the current time, so the sample data never goes stale.

### Front end

```
frontend/src/
  types/       TypeScript types matching the API DTOs
  api/         A shared fetch client (turns ProblemDetails into ApiError) and one module per resource
  components/  Layout with navigation and the create reservation dialog
  pages/       Rooms, Users and Reservations pages
```

Pages never call `fetch` directly; they use the `api/` functions. All calculations (active reservation counts, rule checks) happen in the back end – the front end only checks that the form is filled in and shows the back end's messages.

## Design decisions

- **Active reservation** – a reservation with status `Booked` whose end time has not passed yet. This is used for the user's active reservation count.
- **Ending a meeting early** frees the room from that moment: overlap checks use `ActualEndDate` when it is set, not the originally booked end time. `ActualEndDate` is also what makes it possible to compare booked time with actual use.
- **Overlapping** means one reservation starts before the other ends and ends after the other starts, so back-to-back meetings (11:00–12:00 after 09:00–11:00) are allowed.
- **Times** are stored and exchanged in UTC (ISO 8601). The front end converts them to the user's local time.
